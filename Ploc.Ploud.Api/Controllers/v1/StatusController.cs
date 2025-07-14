using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class StatusController : ApiControllerBase
    {
        [HttpGet]
        [HttpHead]
        public IActionResult Get()
        {
            long ticks = Stopwatch.GetTimestamp();
            var uptime = ((double)ticks) / Stopwatch.Frequency;
            var uptimeSpan = TimeSpan.FromSeconds(uptime);

            var status = new
            {
                Status = Config.Success,
                Uptime = uptimeSpan,
                Host = Environment.MachineName,
                Url = string.Concat(this.Request.Scheme, "://", this.Request.Host, "/"),
                Now = DateTime.Now,
                UtcNow = DateTime.UtcNow
            };

            return this.Ok(status);
        }
    }
}
