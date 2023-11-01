using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

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
            String apiUrl = String.Format("{0}://{1}/{2}/", httpContext.Request.Scheme, httpContext.Request.Host, Config.CurrentVersion);
            var success = new
            {
                Status = Config.Success,
                Version = GetType().Assembly.GetName().Version.ToString(),
                Endpoint = apiUrl,
                GitHub = "https://github.com/PLOC-app/ploc-ploud-server/"
            };
            return Ok(success);
        }
    }
}
