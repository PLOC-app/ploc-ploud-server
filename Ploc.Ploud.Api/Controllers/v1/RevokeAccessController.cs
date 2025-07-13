using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ploc.Ploud.Library;
using System;
using System.Web;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class RevokeAccessController : ApiControllerBase
    {
        private readonly PloudSettings ploudSettings;

        public RevokeAccessController(IOptions<PloudSettings> options)
        {
            this.ploudSettings = options.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(this.ploudSettings.HmacKey) | this.ploudSettings.PublicKey == Guid.Empty)
            {
                return this.BadRequest();
            }

            long timestamp = DateTime.UtcNow.GetSecondsSince1970();
            string signature = string.Concat(this.ploudSettings.PublicKey, timestamp, Config.Actions.Revoke)
                .HMac(this.ploudSettings.HmacKey);

            string link = string.Format("https://app.PLOC.pro/Services/PLOUD/?{0}=1&Timestamp={1}&Signature={2}&App={3}", Config.Actions.Revoke, timestamp, HttpUtility.UrlEncode(signature), this.ploudSettings.PublicKey);
            
            if (Request.Query.ContainsKey("print") | Request.Query.ContainsKey("json"))
            {
                return this.Ok(new
                {
                    Status = Config.Success,
                    Link = link
                });
            }

            return this.Redirect(link);
        }
    }
}
