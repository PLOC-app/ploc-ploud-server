using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ploc.Ploud.Library;
using System;
using System.Web;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class GrantAccessController : ApiControllerBase
    {
        private readonly PloudSettings ploudSettings;

        public GrantAccessController(IOptions<PloudSettings> options)
        {
            this.ploudSettings = options.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if ((String.IsNullOrEmpty(this.ploudSettings.HmacKey))
                | (this.ploudSettings.PublicKey == Guid.Empty))
            {
                return BadRequest();
            }
            long timestamp = DateTime.UtcNow.GetSecondsSince1970();
            String signature = String.Concat(this.ploudSettings.PublicKey, timestamp, Config.Actions.Grant).HMac(this.ploudSettings.HmacKey);
            String link = String.Format("https://app.PLOC.pro/Services/PLOUD/?{0}=1&Timestamp={1}&Signature={2}&App={3}", Config.Actions.Grant, timestamp, HttpUtility.UrlEncode(signature), this.ploudSettings.PublicKey);
            if ((Request.Query.ContainsKey("print"))
                | (Request.Query.ContainsKey("json")))
            {
                return Ok(new
                {
                    Status = Config.Success,
                    Link = link
                });
            }
            return Redirect(link);
        }
    }
}
