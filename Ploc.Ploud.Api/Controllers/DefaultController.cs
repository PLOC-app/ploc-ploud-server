using Microsoft.AspNetCore.Mvc;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("")]
    [Route("v1")]
    [ApiController]
    public class DefaultController : ApiControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var status = new
            {
                Status = Config.Success,
                Feedback = "lapaire@PLOC.co",
                Github = "https://github.com/PLOC-app/ploc-ploud-server/",
                Website = "https://www.PLOC.co/"
            };

            return this.Ok(status);
        }
    }
}
