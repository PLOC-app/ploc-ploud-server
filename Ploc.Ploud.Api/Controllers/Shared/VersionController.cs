using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VersionController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            HttpContext httpContext = Request.HttpContext;
            
            string apiUrl = this.GetApiUrlFromHttpContext(httpContext);
            
            var success = new
            {
                Status = Config.Success,
                Version = GetType().Assembly.GetName().Version.ToString(),
                Endpoint = apiUrl,
                GitHub = "https://github.com/PLOC-app/ploc-ploud-server/"
            };

            return this.Ok(success);
        }

        private string GetApiUrlFromHttpContext(HttpContext httpContext)
        {
            return string.Format("{0}://{1}/{2}/", httpContext.Request.Scheme, httpContext.Request.Host, Config.CurrentVersion);
        }
    }
}
