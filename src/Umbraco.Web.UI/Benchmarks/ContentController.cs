using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Web.Common.Controllers;

namespace Umbraco.Cms.Web.UI.Benchmarks
{
    public class ContentController : UmbracoApiController
    {
        private readonly IContentService _contentService;

        public ContentController(IContentService contentService) => _contentService = contentService;

        [HttpGet]
        public bool ContentLifecycleSingle()
        {
            var content = _contentService.Create("Benchmark", -1, "test");
            _ = _contentService.SaveAndPublish(content);
            return true;
        }

        [HttpGet]
        public bool ContentLifecycleMulti(int count)
        {
            var contentItems = new List<IContent>();
            for (int i = 0; i < count; i++)
            {
                contentItems.Add(_contentService.Create("Benchmark", -1, "test"));
            }
            foreach (var content in contentItems)
            {
                _ = _contentService.SaveAndPublish(content);
            }
            return true;
        }

        [HttpGet]
        public bool BulkContentLifecycleSingle()
        {
            var content = _contentService.Create("Benchmark", -1, "test");
            _ = _contentService.SaveAndPublish(content);
            return true;
        }

        [HttpGet]
        public bool BulkContentLifecycleMulti(int count)
        {
            var contentItems = new List<IContent>();
            for (int i = 0; i < count; i++)
            {
                contentItems.Add(_contentService.Create("Benchmark", -1, "test"));
            }
            _ = _contentService.BulkSaveAndPublish(contentItems);
            return true;
        }

        [HttpGet]
        public string Clean()
        {
            var contentNodes = _contentService.GetRootContent();
            foreach (var node in contentNodes)
            {
                _contentService.Delete(node);
            }
            return "done";
        }

        [HttpGet]
        public string BulkClean()
        {
            var contentNodes = _contentService.GetRootContent();
            _contentService.BulkDelete(contentNodes);
            return "done";
        }
    }
}
