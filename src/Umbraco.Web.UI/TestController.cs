using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Umbraco.Cms.Persistence.EFCore;
using Umbraco.Cms.Web.Common.Controllers;

namespace Umbraco.Cms.Web.UI
{
    public class TestController : UmbracoApiController
    {
        private readonly UmbracoDbContext _umbracoDbContext;

        public TestController(UmbracoDbContext umbracoDbContext)
        {
            _umbracoDbContext = umbracoDbContext;
        }

        [HttpGet]
        public string? Hello()
        {
            return _umbracoDbContext.Database.GetConnectionString();
        }
    }
}
