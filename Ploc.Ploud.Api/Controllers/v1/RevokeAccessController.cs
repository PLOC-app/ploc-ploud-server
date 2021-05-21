using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ploc.Ploud.Library;
using System;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class RevokeAccessController : ControllerBase
    {
        private readonly PloudSettings ploudSettings;

        public RevokeAccessController(IOptions<PloudSettings> options)
        {
            this.ploudSettings = options.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if((String.IsNullOrEmpty(this.ploudSettings.HmacKey))
                | (this.ploudSettings.PublicKey == Guid.Empty))
            {
                return BadRequest();
            }
            long timestamp = DateTime.UtcNow.LongValue();
            String signature = String.Concat(this.ploudSettings.PublicKey, timestamp).HMac(this.ploudSettings.HmacKey);
            String link = String.Format("https://app.PLOC.pro/Services/PLOUD/?Action=Revoke&Timestamp={0}&Signature={1}&App={2}", timestamp, signature, this.ploudSettings.PublicKey);
            return Redirect(link);
        }
    }
}
