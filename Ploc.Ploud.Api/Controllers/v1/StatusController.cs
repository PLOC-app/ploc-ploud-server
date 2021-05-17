﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            long ticks = Stopwatch.GetTimestamp();
            var uptime = ((double)ticks) / Stopwatch.Frequency;
            var uptimeSpan = TimeSpan.FromSeconds(uptime);
            var status = new
            {
                Status = Config.Success,
                Uptime = uptimeSpan,
                Server = Environment.MachineName,
                Now = DateTime.Now,
                UtcNow = DateTime.UtcNow
            };
            return Ok(status);
        }
    }
}
