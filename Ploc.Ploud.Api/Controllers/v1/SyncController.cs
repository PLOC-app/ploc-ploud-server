using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Ploc.Ploud.Library;
using System.IO;
using Microsoft.Extensions.Logging;

namespace Ploc.Ploud.Api.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IMemoryCache memoryCache;
        private readonly IAuthenticationService authenticationService;
        private readonly ISignatureService signatureService;
        private readonly ISyncService syncService;
        private readonly INotificationService notificationService;
        private readonly ILogger logger;
        private readonly PloudSettings ploudSettings;

        public SyncController(IMemoryCache memoryCache, 
            IAuthenticationService authenticationService, 
            ISignatureService signatureService, 
            ISyncService syncService,
            INotificationService notificationService,
            ILogger<SyncController> logger,
            IOptions<PloudSettings> options)
        {
            this.memoryCache = memoryCache;
            this.authenticationService = authenticationService;
            this.signatureService = signatureService;
            this.notificationService = notificationService;
            this.syncService = syncService;
            this.logger = logger;
            this.ploudSettings = options.Value;
        }

        [Route("Sync")]
        [HttpPost]
        public async Task<IActionResult> Sync(SyncRequest request)
        {
            if(request == null)
            {
                this.logger.LogWarning("Sync(), Request = NULL");
                return BadRequest(new
                {
                    Error = ValidationStatus.InvalidParams
                });
            }

            this.logger.LogInformation("Sync.Begin(), Token = {token}, LastSyncTime = {lastSyncTime}, Objects = {objectCount}",
                request.Token,
                request.LastSyncTime,
                request.Objects == null ? -1 : request.Objects.Count);

            ValidationStatus validationStatus = request.Validate();
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Sync.Validate(), Status = {validationStatus}", validationStatus);
                return BadRequest(new
                {
                    Error = validationStatus
                });
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Sync);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                if ((signatureResponse == null)
                    || (!signatureResponse.IsValid))
                {
                    this.logger.LogWarning("Sync.VerifySignature(), Signature = {0}", signatureRequest.Signature);
                    return Forbid();
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            if((authenticationResponse == null)
                || (!authenticationResponse.IsAuthenticated))
            {
                this.logger.LogWarning("Sync.Authenticate(), Token = {0}", authenticationRequest.Token);
                return Forbid();
            }

            String ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            if((String.IsNullOrEmpty(ploudFilePath))
                || (!System.IO.File.Exists(ploudFilePath)))
            {
                this.logger.LogWarning("Sync.GetPloudFilePath(), PloudFilePath = {ploudFilePath}", ploudFilePath);
                return NotFound(new
                {
                    Error = ValidationStatus.PloudNotInitialized
                });
            }

            String ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            String downloadFileUrlFormat = String.Format("{0}v1/sync/documents/{{0}}", this.ploudSettings.Url);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath, downloadFileUrlFormat);
            SyncResponse syncResponse = await request.SynchronizeAsync(this.syncService, syncSettings);
            if(syncResponse == null)
            {
                this.logger.LogError("Sync.Synchronize(), PloudFilePath = {ploudFilePath}", ploudFilePath);
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = ValidationStatus.PloudNotInitialized
                });
            }

            if((request.Objects != null) 
                && (request.Objects.Count > 0))
            {
                NotificationRequest notificationRequest = request.ToNotificationRequest(this.ploudSettings.PublicKey);
                await notificationRequest.NotifyAsync(this.notificationService);
            }

            return Ok(new
            {
                Status = Config.Success,
                Objects = syncResponse.Objects
            });
        }

        [Route("Initialize")]
        [HttpPost]
        public async Task<IActionResult> Initialize(InitializeRequest request)
        {
            if ((request == null)
                || (request.File == null))
            {
                this.logger.LogWarning("Initialize(), Request = NULL");
                return BadRequest(new
                {
                    Error = ValidationStatus.InvalidParams
                });
            }

            this.logger.LogInformation("Initialize.Begin(), Token = {token}", request.Token);

            ValidationStatus validationStatus = request.Validate();
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Initialize.Validate(), Status = {validationStatus}", validationStatus);
                return BadRequest(new
                {
                    Error = validationStatus
                });
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Initialize);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                if ((signatureResponse == null)
                    || (!signatureResponse.IsValid))
                {
                    this.logger.LogWarning("Initialize.VerifySignature(), Signature = {0}", signatureRequest.Signature);
                    return Forbid();
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            if ((authenticationResponse == null)
                || (!authenticationResponse.IsAuthenticated))
            {
                this.logger.LogWarning("Initialize.Authenticate(), Token = {0}", authenticationRequest.Token);
                return Forbid();
            }

            String ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            if (System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Initialize.GetPloudFilePath(PloudAlreadyInitialized), PloudFilePath = {ploudFilePath}", ploudFilePath);
                return BadRequest(new
                {
                    Error = ValidationStatus.PloudAlreadyInitialized
                });
            }

            String ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            bool success = await request.InitializeAsync(this.syncService, syncSettings);
            if (!success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = ValidationStatus.ServerError
                });
            }

            return Ok(new
            {
                Status = Config.Success
            });
        }

        [Route("Uninitialize")]
        [HttpPost]
        public async Task<IActionResult> Uninitialize(UninitializeRequest request)
        {
            if (request == null)
            {
                this.logger.LogWarning("Unitialize(), Request = NULL");
                return BadRequest();
            }

            this.logger.LogInformation("Unitialize.Begin(), Token = {token}", request.Token);

            ValidationStatus validationStatus = request.Validate();
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Unitialize.Validate(), Status = {validationStatus}", validationStatus);
                return BadRequest(new
                {
                    Error = validationStatus
                });
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Initialize);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                if ((signatureResponse == null)
                    || (!signatureResponse.IsValid))
                {
                    this.logger.LogWarning("Unitialize.VerifySignature(), Signature = {0}", signatureRequest.Signature);
                    return Forbid();
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            if ((authenticationResponse == null)
                || (!authenticationResponse.IsAuthenticated))
            {
                this.logger.LogWarning("Unitialize.Authenticate(), Token = {0}", authenticationRequest.Token);
                return Forbid();
            }

            String ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            if (!System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Uninitialize.GetPloudFilePath(PloudNotInitialized), PloudFilePath = {ploudFilePath}", ploudFilePath);
                return BadRequest(new
                {
                    Error = ValidationStatus.PloudNotInitialized
                });
            }

            String ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            bool success = await request.UninitializeAsync(this.syncService, syncSettings);
            if (!success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    Error = ValidationStatus.ServerError
                });
            }
            return Ok(new
            {
                Status = Config.Success
            });
        }

        [Route("documents/{0}/")]
        [HttpPost]
        public async Task<IActionResult> GetDocument(DocumentRequest request)
        {
            if ((request == null)
                || (String.IsNullOrWhiteSpace(request.Document)))
            {
                this.logger.LogWarning("GetDocument(), Request = NULL");
                return BadRequest();
            }

            this.logger.LogInformation("GetDocument.Begin(), Token = {token}, Document = {document}", request.Token, request.Document);

            ValidationStatus validationStatus = request.Validate();
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("GetDocument.Validate(), Status = {validationStatus}", validationStatus);
                return BadRequest(new
                {
                    Error = validationStatus
                });
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Initialize);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                if ((signatureResponse == null)
                    || (!signatureResponse.IsValid))
                {
                    this.logger.LogWarning("GetDocument.VerifySignature(), Signature = {0}", signatureRequest.Signature);
                    return Forbid();
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            if ((authenticationResponse == null)
                || (!authenticationResponse.IsAuthenticated))
            {
                this.logger.LogWarning("GetDocument.Authenticate(), Token = {0}", authenticationRequest.Token);
                return Forbid();
            }

            String ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            if (!System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Uninitialize.GetPloudFilePath(PloudNotInitialized), PloudFilePath = {ploudFilePath}", ploudFilePath);
                return BadRequest(new
                {
                    Error = ValidationStatus.PloudNotInitialized
                });
            }

            String ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            Document document = await request.GetDocumentAsync(this.syncService, syncSettings);
            if ((document == null)
                || (document.Data == null))
            {
                return StatusCode(StatusCodes.Status404NotFound, new
                {
                    Error = ValidationStatus.InvalidParams
                });
            }
            return File(document.Data, "application/octet-stream", "PLOUD.file");
        }

        [Route("status")]
        [HttpPost]
        public async Task<IActionResult> Status(StatusRequest request)
        {
            if (request == null)
            {
                this.logger.LogWarning("Status(), Request = NULL");
                return BadRequest();
            }

            this.logger.LogInformation("Status.Begin(), Token = {token}", request.Token);

            ValidationStatus validationStatus = request.Validate();
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Status.Validate(), Status = {validationStatus}", validationStatus);
                return BadRequest(new
                {
                    Error = validationStatus
                });
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Initialize);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                if ((signatureResponse == null)
                    || (!signatureResponse.IsValid))
                {
                    this.logger.LogWarning("Status.VerifySignature(), Signature = {0}", signatureRequest.Signature);
                    return Forbid();
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            if ((authenticationResponse == null)
                || (!authenticationResponse.IsAuthenticated))
            {
                this.logger.LogWarning("Status.Authenticate(), Token = {0}", authenticationRequest.Token);
                return Forbid();
            }

            String ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            String ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath); 
            
            return Ok(new
            {
                Status = Config.Success,
                Enabled = syncSettings.IsPloudEnabled
            });
        }
    }
}
