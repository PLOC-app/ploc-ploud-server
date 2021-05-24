using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Ploc.Ploud.Library;
using System;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SetupController : ControllerBase
    {
        private readonly PloudSettings ploudSettings;

        public SetupController(IOptions<PloudSettings> options)
        {
            this.ploudSettings = options.Value;
        }

        [Route("Rsa/Export")]
        [HttpGet]
        public async Task<IActionResult> ExportRsaKey([FromQuery]RsaKeyRequest request)
        {
            if (String.IsNullOrEmpty(this.ploudSettings.PrivateKey))
            {
                return BadRequest();
            }
            if ((request == null)
                || (String.IsNullOrEmpty(request.PrivateKey)))
            {
                return BadRequest();
            }
            if (this.ploudSettings.PrivateKey != request.PrivateKey)
            {
                await Task.Delay(2000); // To avoid brute force attacks
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    Status = Config.Error
                });
            }
            using (ICryptoProvider cryptoProvider = ICryptoProviderFactory.CreateProvider())
            {
                String data = cryptoProvider.ExportRsaKey();
                var success = new
                {
                    Status = Config.Success,
                    Data = data
                };
                return Ok(success);
            }
        }

        [Route("Rsa/Import")]
        [HttpPost]
        public async Task<IActionResult> ImportRsaKey([FromBody]RsaKeyRequest request)
        {
            if (String.IsNullOrEmpty(this.ploudSettings.PrivateKey))
            {
                return BadRequest();
            }
            if ((request == null)
                || (String.IsNullOrEmpty(request.PrivateKey))
                || (String.IsNullOrEmpty(request.Data)))
            {
                return BadRequest();
            }
            if (this.ploudSettings.PrivateKey != request.PrivateKey)
            {
                await Task.Delay(2000); // To avoid brute force attacks
                return StatusCode(StatusCodes.Status403Forbidden, new
                {
                    Status = Config.Error
                });
            }
            using (ICryptoProvider cryptoProvider = ICryptoProviderFactory.CreateProvider())
            {
                bool status = cryptoProvider.ImportRsaKey(request.Data);
                var success = new
                {
                    Status = status ? Config.Success : Config.Error
                };
                return Ok(success);
            }
        }
    }
}
