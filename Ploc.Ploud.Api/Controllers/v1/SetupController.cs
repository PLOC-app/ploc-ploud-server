using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ploc.Ploud.Library;
using System;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SetupController : ApiControllerBase
    {
        private readonly ILogger<SetupController> logger;
        private readonly PloudSettings ploudSettings;

        public SetupController(
            ILogger<SetupController> logger,
            IOptions<PloudSettings> options)
        {
            this.logger = logger;
            this.ploudSettings = options.Value;
        }

        [Route("Rsa/Export")]
        [HttpGet]
        public async Task<IActionResult> ExportRsaKey([FromQuery] RsaKeyRequest request)
        {
            if ((request == null)
                || (String.IsNullOrEmpty(request.PrivateKey)))
            {
                this.logger.LogWarning("ExportRsaKey(), Request = NULL");
                return BadRequest();
            }
            this.logger.LogInformation("ExportRsaKey.Start()");
            if (String.IsNullOrEmpty(this.ploudSettings.PrivateKey))
            {
                this.logger.LogError("ExportRsaKey(), Private key is not defined.");
                return BadRequest();
            }
            if (this.ploudSettings.PrivateKey != request.PrivateKey)
            {
                this.logger.LogError("ExportRsaKey(), Private key is incorrect.");
                await Task.Delay(2000); // To avoid brute force attacks
                return Forbid();
            }
            this.logger.LogInformation("ExportRsaKey.Start()");
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
        public async Task<IActionResult> ImportRsaKey([FromBody] RsaKeyRequest request)
        {
            if ((request == null)
                || (String.IsNullOrEmpty(request.PrivateKey))
                || (String.IsNullOrEmpty(request.Data)))
            {
                this.logger.LogWarning("ImportRsaKey(), Request = NULL");
                return BadRequest();
            }
            this.logger.LogInformation("ImportRsaKey.Start()");
            if (String.IsNullOrEmpty(this.ploudSettings.PrivateKey))
            {
                this.logger.LogError("ExportRsaKey(), Private key is not defined.");
                return BadRequest();
            }
            if (this.ploudSettings.PrivateKey != request.PrivateKey)
            {
                this.logger.LogError("ExportRsaKey(), Private key is incorrect.");
                await Task.Delay(2000); // To avoid brute force attacks
                return Forbid();
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
