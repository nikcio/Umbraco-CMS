using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Web.Caching;
using SixLabors.ImageSharp.Web.DependencyInjection;
using SixLabors.ImageSharp.Web.Middleware;
using SixLabors.ImageSharp.Web.Providers;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Media;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Security;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Imaging.ImageSharp.ImageProcessors;
using Umbraco.Cms.Imaging.ImageSharp.Media;
using Umbraco.Cms.Web.Common.ApplicationBuilder;
using Umbraco.Extensions;

namespace Umbraco.Cms.Imaging.ImageSharp;

public static class UmbracoBuilderExtensions
{
    /// <summary>
    ///     Adds Image Sharp with Umbraco settings
    /// </summary>
    public static IServiceCollection AddUmbracoImageSharp(this IUmbracoBuilder builder)
    {
        // Add default ImageSharp configuration and service implementations
        builder.Services.AddSingleton(Configuration.Default);
        builder.Services.AddUnique<IImageDimensionExtractor, ImageSharpDimensionExtractor>();

        builder.Services.AddSingleton<IImageUrlGenerator, ImageSharpImageUrlGenerator>();

        builder.Services.AddImageSharp()
            // Replace default image provider
            .ClearProviders()
            .AddProvider<WebRootImageProvider>()
            // Add custom processors
            .AddProcessor<CropWebProcessor>();

        // Configure middleware
        builder.Services.AddTransient<IConfigureOptions<ImageSharpMiddlewareOptions>, ConfigureImageSharpMiddlewareOptions>();

        // Configure cache options
        builder.Services.AddTransient<IConfigureOptions<PhysicalFileSystemCacheOptions>, ConfigurePhysicalFileSystemCacheOptions>();

        // Important we handle image manipulations before the static files, otherwise the querystring is just ignored
        builder.Services.Configure<UmbracoPipelineOptions>(options =>
        {
            options.AddFilter(new UmbracoPipelineFilter(nameof(ImageSharpComposer))
            {
                PrePipeline = prePipeline =>
                {
                    prePipeline.UseMiddleware<UmbracoImageSharpMiddleware>();
                    prePipeline.UseImageSharp();
                }
            });
        });

        return builder.Services;
    }

    public class UmbracoImageSharpMiddleware
    {
        private readonly RequestDelegate _next;

        public UmbracoImageSharpMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Path.Value != null && (httpContext.Request.Path.Value.InvariantEndsWith(".deleted") || httpContext.Request.Path.Value.InvariantEndsWith(".deleted/")))
            {
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            await _next(httpContext);
        }
    }

    public class UmbracoImageComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<MediaMovingToRecycleBinNotification, UmbracoImageNotificationHandler>();
            builder.AddNotificationHandler<MediaMovingNotification, UmbracoImageNotificationHandler>();
        }
    }

    public class UmbracoImageNotificationHandler : INotificationHandler<MediaMovingToRecycleBinNotification>, INotificationHandler<MediaMovingNotification>
    {
        private readonly MediaFileManager _mediaFileManager;

        private readonly IMediaService _mediaService;

        public UmbracoImageNotificationHandler(MediaFileManager mediaFileManager, IMediaService mediaService)
        {
            _mediaFileManager = mediaFileManager;
            _mediaService = mediaService;
        }

        public void Handle(MediaMovingToRecycleBinNotification notification)
        {
            foreach (var mediaItem in notification.MoveInfoCollection)
            {
                HandleMediaItem(mediaItem, true);
            }
        }

        public void Handle(MediaMovingNotification notification)
        {
            foreach (var mediaItem in notification.MoveInfoCollection)
            {
                HandleMediaItem(mediaItem, false);
            }
        }

        private void HandleMediaItem(MoveEventInfo<IMedia> mediaItem, bool trashing)
        {
            if(mediaItem.Entity.ContentType.Name == Constants.Conventions.MediaTypes.Folder)
            {
                var page = 0;
                var pageSize = 10;
                long totalRecords;
                do
                {
                     var children = _mediaService.GetPagedChildren(mediaItem.Entity.Id, page, pageSize, out totalRecords);

                    foreach(var child in children)
                    {
                        HandleMediaItem(new MoveEventInfo<IMedia>(child, child.Path, child.ParentId), trashing);
                    }

                    page++;

                } while (page * pageSize < totalRecords);
                return;
            }

            if (mediaItem.Entity.Trashed && mediaItem.NewParentId != Constants.System.RecycleBinMedia)
            {
                var umbracoFileValue = mediaItem.Entity.GetValue<string>(Constants.Conventions.Media.File);

                if (umbracoFileValue == null)
                {
                    return;
                }

                var umbracoFile = JsonConvert.DeserializeObject<UmbracoFileValue>(umbracoFileValue);

                if (umbracoFile == null)
                {
                    return;
                }

                var umbracoFilePath = _mediaFileManager.FileSystem.GetFullPath(umbracoFile.Src);

                if (_mediaFileManager.FileSystem.FileExists(umbracoFilePath) && !_mediaFileManager.FileSystem.FileExists(umbracoFilePath[..^".deleted".Length]))
                {
                    _mediaFileManager.FileSystem.CopyFile(umbracoFilePath, umbracoFilePath[..^".deleted".Length]);

                    _mediaFileManager.FileSystem.DeleteFile(umbracoFilePath);

                    umbracoFile.Src = umbracoFile.Src[..^".deleted".Length];
                    mediaItem.Entity.SetValue(Constants.Conventions.Media.File, JsonConvert.SerializeObject(umbracoFile));

                    _mediaService.Save(mediaItem.Entity);
                }
            }
            else if(mediaItem.NewParentId == Constants.System.RecycleBinMedia || trashing)
            {
                var umbracoFileValue = mediaItem.Entity.GetValue<string>(Constants.Conventions.Media.File);

                if (umbracoFileValue == null)
                {
                    return;
                }

                var umbracoFile = JsonConvert.DeserializeObject<UmbracoFileValue>(umbracoFileValue);

                if (umbracoFile == null)
                {
                    return;
                }

                var umbracoFilePath = _mediaFileManager.FileSystem.GetFullPath(umbracoFile.Src);

                if (_mediaFileManager.FileSystem.FileExists(umbracoFilePath))
                {
                    if (_mediaFileManager.FileSystem.FileExists(umbracoFilePath + ".deleted"))
                    {
                        _mediaFileManager.FileSystem.DeleteFile(umbracoFilePath + ".deleted");
                    }

                    _mediaFileManager.FileSystem.CopyFile(umbracoFilePath, umbracoFilePath + ".deleted");

                    _mediaFileManager.FileSystem.DeleteFile(umbracoFilePath);

                    umbracoFile.Src = umbracoFile.Src + ".deleted";
                    mediaItem.Entity.SetValue(Constants.Conventions.Media.File, JsonConvert.SerializeObject(umbracoFile));

                    _mediaService.Save(mediaItem.Entity);
                }
            }
        }

        private class UmbracoFileValue
        {
            public string Src { get; set; } = "";
        }
    }
}
