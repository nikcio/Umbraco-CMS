﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Umbraco.Cms.Infrastructure.WebAssets {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Umbraco.Cms.Infrastructure.WebAssets.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///    
        ///    &apos;lib/jquery/jquery.min.js&apos;,
        ///    &apos;lib/jquery-ui/jquery-ui.min.js&apos;,
        ///    &apos;lib/jquery-ui-touch-punch/jquery.ui.touch-punch.min.js&apos;,
        ///
        ///    &apos;lib/angular/angular.min.js&apos;,
        ///    &apos;lib/underscore/underscore-min.js&apos;,
        ///
        ///    &apos;lib/moment/moment.min.js&apos;,
        ///    &apos;lib/flatpickr/flatpickr.min.js&apos;,
        ///
        ///    &apos;lib/animejs/anime.min.js&apos;,
        ///
        ///    &apos;lib/angular-route/angular-route.min.js&apos;,
        ///    &apos;lib/angular-cookies/angular-cookies.min.js&apos;,
        ///    &apos;lib/angular-aria/angular-aria.min.js&apos;,
        ///    &apos;lib/angular-touch/angular-touch [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string JsInitialize {
            get {
                return ResourceManager.GetString("JsInitialize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to LazyLoad.js(&quot;##JsInitialize##&quot;, function () {
        ///    //we need to set the legacy UmbClientMgr path
        ///    if ((typeof UmbClientMgr) !== &quot;undefined&quot;) {
        ///        UmbClientMgr.setUmbracoPath(&apos;&quot;##UmbracoPath##&quot;&apos;);
        ///    }
        ///
        ///    jQuery(document).ready(function () {
        ///
        ///        angular.bootstrap(document, [&apos;&quot;##AngularModule##&quot;&apos;]);
        ///
        ///    });
        ///});
        ///.
        /// </summary>
        internal static string Main {
            get {
                return ResourceManager.GetString("Main", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///    &apos;lib/jquery/jquery.min.js&apos;,
        ///    &apos;lib/angular/angular.min.js&apos;,
        ///    &apos;lib/underscore/underscore-min.js&apos;,
        ///    &apos;lib/umbraco/Extensions.js&apos;,
        ///    &apos;js/utilities.min.js&apos;,
        ///    &apos;js/app.min.js&apos;,
        ///    &apos;js/umbraco.resources.min.js&apos;,
        ///    &apos;js/umbraco.services.min.js&apos;,
        ///    &apos;js/umbraco.interceptors.min.js&apos;,
        ///    &apos;ServerVariables&apos;,
        ///    &apos;lib/signalr/signalr.min.js&apos;,
        ///    &apos;js/umbraco.preview.min.js&apos;
        ///]
        ///.
        /// </summary>
        internal static string PreviewInitialize {
            get {
                return ResourceManager.GetString("PreviewInitialize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to  // TODO: This would be nicer as an angular module so it can be injected into stuff... that&apos;d be heaps nicer, but
        /// // how to do that when this is not a regular JS file, it is a server side JS file and RequireJS seems to only want
        /// // to force load JS files ?
        /// 
        /// //create the namespace (NOTE: This loads before any dependencies so we don&apos;t have a namespace mgr so we just create it manually)
        ///var Umbraco = {};
        ///Umbraco.Sys = {};
        /// //define a global static object
        ///Umbraco.Sys.ServerVariables = ##Variables##  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ServerVariables {
            get {
                return ResourceManager.GetString("ServerVariables", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [
        ///  &apos;lib/tinymce/tinymce.min.js&apos;,
        ///
        ///  &apos;lib/tinymce/plugins/anchor/plugin.min.js&apos;,
        ///  &apos;lib/tinymce/plugins/charmap/plugin.min.js&apos;,
        ///  &apos;lib/tinymce/plugins/table/plugin.min.js&apos;,
        ///  &apos;lib/tinymce/plugins/lists/plugin.min.js&apos;,
        ///  &apos;lib/tinymce/plugins/advlist/plugin.min.js&apos;,
        ///  &apos;lib/tinymce/plugins/autolink/plugin.min.js&apos;,
        ///  &apos;lib/tinymce/plugins/directionality/plugin.min.js&apos;,
        ///  &apos;lib/tinymce/plugins/searchreplace/plugin.min.js&apos;
        ///]
        ///.
        /// </summary>
        internal static string TinyMceInitialize {
            get {
                return ResourceManager.GetString("TinyMceInitialize", resourceCulture);
            }
        }
    }
}
