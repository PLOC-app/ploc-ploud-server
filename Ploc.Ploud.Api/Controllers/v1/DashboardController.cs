using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ploc.Ploud.Library;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class DashboardController : ApiControllerBase
    {
        private readonly IDashboardService dashboardService;
        private readonly IMemoryCache memoryCache;
        private readonly IAuthenticationService authenticationService;
        private readonly ISignatureService signatureService;
        private readonly PloudSettings ploudSettings;
        private readonly ILogger<DashboardController> logger;

        public DashboardController(
            IDashboardService dashboardService,
            IMemoryCache memoryCache,
            IAuthenticationService authenticationService,
            ISignatureService signatureService,
            ILogger<DashboardController> logger, 
            IOptions<PloudSettings> options)
        {
            this.dashboardService = dashboardService;
            this.memoryCache = memoryCache;
            this.authenticationService = authenticationService;
            this.signatureService = signatureService;
            this.logger = logger;
            this.ploudSettings = options.Value;
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Get(RequestBase request)
        {
            if (request == null)
            {
                this.logger.LogWarning("Dashboard(), Request = NULL");
                return BadRequest();
            }

            this.logger.LogInformation("Dashboard.Start(), Token = {Token}", request.Token);
            ValidationStatus validationStatus = request.Validate();
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Dashboard.Validate(), Status = {ValidationStatus}", validationStatus);
                return BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.GetDashboard);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                if ((signatureResponse == null)
                    || (!signatureResponse.IsValid))
                {
                    this.logger.LogWarning("Dashboard.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);
                    this.logger.LogWarning("Dashboard.VerifySignature(), Request = {Request}", JsonSerializer.Serialize(signatureRequest));
                    return Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            if ((authenticationResponse == null)
                || (!authenticationResponse.IsAuthenticated))
            {
                this.logger.LogWarning("Dashboard.Authenticate(), Token = {Token}", authenticationRequest.Token);
                return Forbid(ValidationStatus.InvalidToken);
            }

            String ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            if (!System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Dashboard.GetPloudFilePath(PloudNotInitialized), PloudFilePath = {PloudFilePath}", ploudFilePath);
                return BadRequest(ValidationStatus.PloudNotInitialized);
            }

            String ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            Dashboard dashboard = await request.GetDashboardAsync(this.dashboardService, syncSettings);
            if (dashboard == null)
            {
                return InternalServerError(ValidationStatus.ServerError);
            }
            return Ok(new
            {
                Status = Config.Success,
                Dashboard = dashboard
            });
        }
    }
}
