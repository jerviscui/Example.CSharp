using Com.Ctrip.Framework.Apollo.OpenApi;
using Com.Ctrip.Framework.Apollo.OpenApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace ApolloTestApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OpenApiTestController : ControllerBase
    {
        [HttpGet(Name = "CreateNamespace")]
        public async Task<IActionResult> CreateNamespace()
        {
            var apiFactory = new OpenApiFactory(new OpenApiOptions { PortalUrl = new Uri(""), Token = "" });

            var clusterClient = apiFactory.CreateAppClusterClient("appId");
            var appNamespace = await clusterClient.CreateAppNamespace(new AppNamespace
            {
                Name = "",
                AppId = "appId",
                Format = "json",
                IsPublic = false,
                DataChangeCreatedBy = null
            });

            //var namespaceClient = apiFactory.CreateNamespaceClient("appId", "env", "", "");
            //await namespaceClient.UpdateItem(new Item());

            return Ok();
        }
    }
}
