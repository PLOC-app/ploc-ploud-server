using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Ploc.Ploud.Library;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace Ploc.Ploud.Api.Controllers.v1
{
    [Route("v1/[controller]")]
    [ApiController]
    public class SyncController : ApiControllerBase
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
            IWebHostEnvironment webHostEnvironment,
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
            this.ploudSettings.Directory = Path.Combine(webHostEnvironment.ContentRootPath, Config.Folders.Data);
        }

        [Route("Exec")]
        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Exec([FromForm] SyncRequest request)
        {
            if (request == null)
            {
                this.logger.LogWarning("Sync(), Request = NULL");

                return this.BadRequest(ValidationStatus.InvalidParams);
            }

            this.logger.LogInformation("Sync.Start(), Token = {Token}, LastSyncTime = {LastSyncTime}, Objects = {ObjectCount}",
                request.Token,
                request.LastSyncTime,
                request.Objects == null ? -1 : request.Objects.Count);

            ValidationStatus validationStatus = request.Validate();

            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Sync.Validate(), Status = {ValidationStatus}", validationStatus);

                return this.BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Sync);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                
                if (signatureResponse == null || !signatureResponse.IsValid)
                {
                    this.logger.LogWarning("Sync.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);

                    return this.Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            
            if (authenticationResponse == null || !authenticationResponse.IsAuthenticated)
            {
                this.logger.LogWarning("Sync.Authenticate(), Token = {Token}", authenticationRequest.Token);

                return this.Forbid(ValidationStatus.InvalidToken);
            }

            string ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            
            if (string.IsNullOrEmpty(ploudFilePath) || !System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogWarning("Sync.GetPloudFilePath(), PloudFilePath = {PloudFilePath}", ploudFilePath);

                return this.NotFound(ValidationStatus.PloudNotInitialized);
            }

            string apiUrl = string.Concat(this.Request.Scheme, "://", this.Request.Host, "/");
            string ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            string downloadFileUrlFormat = string.Format("{0}v1/sync/documents/{{0}}", apiUrl);

            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath, downloadFileUrlFormat);
            SyncResponse syncResponse = await request.SynchronizeAsync(this.syncService, syncSettings);
            
            if (syncResponse == null)
            {
                this.logger.LogError("Sync.Synchronize(), SyncResponse = NULL, PloudFilePath = {PloudFilePath}", ploudFilePath);

                return this.InternalServerError(ValidationStatus.PloudNotInitialized);
            }

            if (request.Objects != null && request.Objects.Count > 0)
            {
                NotificationRequest notificationRequest = request.ToNotificationRequest(this.ploudSettings.PublicKey);
                
                await notificationRequest.NotifyAsync(this.notificationService);
            }

            return this.Ok(new
            {
                Status = Config.Success,
                Objects = syncResponse.Objects
            });
        }

        [Route("Initialize")]
        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> Initialize([FromForm] InitializeRequest request)
        {
            if (request == null || request.File == null)
            {
                request = InitializeRequest.FromHeaders(this.Request);
            }

            if (request == null || request.File == null)
            {
                if (request == null)
                {
                    this.logger.LogWarning("Initialize(), Request = NULL");
                }
                else
                {
                    this.logger.LogWarning("Initialize(), Request.File = NULL");
                }

                return this.BadRequest(ValidationStatus.InvalidParams);
            }

            this.logger.LogInformation("Initialize.Start(), Token = {Token}", request.Token);

            ValidationStatus validationStatus = request.Validate();

            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Initialize.Validate(), Status = {ValidationStatus}", validationStatus);
                
                return this.BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Initialize);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);

                if (signatureResponse == null || !signatureResponse.IsValid)
                {
                    this.logger.LogWarning("Initialize.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);

                    return this.Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);

            if (authenticationResponse == null || !authenticationResponse.IsAuthenticated)
            {
                this.logger.LogWarning("Initialize.Authenticate(), Token = {Token}", authenticationRequest.Token);

                return this.Forbid(ValidationStatus.InvalidToken);
            }

            string ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);

            if (System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Initialize.GetPloudFilePath(PloudAlreadyInitialized), PloudFilePath = {PloudFilePath}", ploudFilePath);

                return this.BadRequest(ValidationStatus.PloudAlreadyInitialized);
            }

            string ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            bool success = await request.InitializeAsync(this.syncService, syncSettings);

            if (!success)
            {
                return this.InternalServerError(ValidationStatus.ServerError);
            }

            return this.Ok(new
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

                return this.BadRequest();
            }

            this.logger.LogInformation("Unitialize.Start(), Token = {Token}", request.Token);

            ValidationStatus validationStatus = request.Validate();
            
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Unitialize.Validate(), Status = {ValidationStatus}", validationStatus);
                return this.BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Uninitialize);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                
                if (signatureResponse == null || !signatureResponse.IsValid)
                {
                    this.logger.LogWarning("Unitialize.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);
                    this.logger.LogWarning("Unitialize.VerifySignature(), Request = {Request}", JsonSerializer.Serialize(signatureRequest));
                    
                    return this.Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            
            if (authenticationResponse == null || !authenticationResponse.IsAuthenticated)
            {
                this.logger.LogWarning("Unitialize.Authenticate(), Token = {Token}", authenticationRequest.Token);

                return this.Forbid(ValidationStatus.InvalidToken);
            }

            string ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            
            if (!System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Uninitialize.GetPloudFilePath(PloudNotInitialized), PloudFilePath = {PloudFilePath}", ploudFilePath);
                return this.BadRequest(ValidationStatus.PloudNotInitialized);
            }

            string ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);

            bool success = await request.UninitializeAsync(this.syncService, syncSettings);
            
            if (!success)
            {
                return this.InternalServerError(ValidationStatus.ServerError);
            }

            return this.Ok(new
            {
                Status = Config.Success
            });
        }

        [Route("EraseData")]
        [HttpPost]
        public async Task<IActionResult> EraseData(EraseDataRequest request)
        {
            if (request == null)
            {
                this.logger.LogWarning("EraseData(), Request = NULL");
                
                return this.BadRequest();
            }

            this.logger.LogInformation("EraseData.Start(), Token = {Token}", request.Token);

            ValidationStatus validationStatus = request.Validate();

            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("EraseData.Validate(), Status = {ValidationStatus}", validationStatus);

                return this.BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.EraseData);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                
                if (signatureResponse == null || !signatureResponse.IsValid)
                {
                    this.logger.LogWarning("EraseData.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);
                    this.logger.LogWarning("EraseData.VerifySignature(), Request = {Request}", JsonSerializer.Serialize(signatureRequest));
                    
                    return this.Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            
            if (authenticationResponse == null || !authenticationResponse.IsAuthenticated)
            {
                this.logger.LogWarning("EraseData.Authenticate(), Token = {Token}", authenticationRequest.Token);

                return this.Forbid(ValidationStatus.InvalidToken);
            }

            string ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            string ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            bool success = await request.EraseDataAsync(this.syncService, syncSettings);

            if (!success)
            {
                return this.InternalServerError(ValidationStatus.ServerError);
            }

            return this.Ok(new
            {
                Status = Config.Success
            });
        }

        [Route("documents/{document}/")]
        [HttpPost]
        public async Task<IActionResult> GetDocument(DocumentRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Document))
            {
                this.logger.LogWarning("GetDocument(), Request = NULL");

                return this.BadRequest();
            }

            this.logger.LogInformation("GetDocument.Start(), Token = {Token}, Document = {Document}", request.Token, request.Document);

            ValidationStatus validationStatus = request.Validate();

            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("GetDocument.Validate(), Status = {ValidationStatus}", validationStatus);

                return this.BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.GetDocument, request.Document);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);

                if (signatureResponse == null || !signatureResponse.IsValid)
                {
                    this.logger.LogWarning("GetDocument.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);
                    
                    return this.Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            
            if (authenticationResponse == null || !authenticationResponse.IsAuthenticated)
            {
                this.logger.LogWarning("GetDocument.Authenticate(), Token = {Token}", authenticationRequest.Token);

                return this.Forbid(ValidationStatus.InvalidToken);
            }

            string ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            
            if (!System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Uninitialize.GetPloudFilePath(PloudNotInitialized), PloudFilePath = {PloudFilePath}", ploudFilePath);
                
                return this.BadRequest(ValidationStatus.PloudNotInitialized);
            }

            string ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            Document document = await request.GetDocumentAsync(this.syncService, syncSettings);

            if (document == null || document.Data == null)
            {
                return this.NotFound(ValidationStatus.InvalidParams);
            }

            return this.File(document.Data, "application/octet-stream", "PLOUD.file");
        }

        [Route("status")]
        [HttpPost]
        public async Task<IActionResult> Status(StatusRequest request)
        {
            if (request == null)
            {
                this.logger.LogWarning("Status(), Request = NULL");

                return this.BadRequest();
            }

            this.logger.LogInformation("Status.Start(), Token = {Token}", request.Token);

            ValidationStatus validationStatus = request.Validate();

            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Status.Validate(), Status = {ValidationStatus}", validationStatus);

                return this.BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Status);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                
                if (signatureResponse == null || !signatureResponse.IsValid)
                {
                    this.logger.LogWarning("Status.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);

                    return this.Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            
            if (authenticationResponse == null || !authenticationResponse.IsAuthenticated)
            {
                this.logger.LogWarning("Status.Authenticate(), Token = {Token}", authenticationRequest.Token);

                return this.Forbid(ValidationStatus.InvalidToken);
            }

            string ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            string ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            
            return this.Ok(new
            {
                Status = Config.Success,
                Enabled = syncSettings.IsPloudEnabled
            });
        }

        [Route("Download")]
        [HttpGet]
        public async Task<IActionResult> Download([FromQuery] DownloadRequest request)
        {
            if (request == null)
            {
                this.logger.LogWarning("Download(), Request = NULL");

                return this.BadRequest(ValidationStatus.InvalidParams);
            }

            this.logger.LogInformation("Download.Start(), Token = {Token}", request.Token);

            ValidationStatus validationStatus = request.Validate();
            
            if (validationStatus != ValidationStatus.Ok)
            {
                this.logger.LogWarning("Download.Validate(), Status = {ValidationStatus}", validationStatus);

                return this.BadRequest(validationStatus);
            }

            if (this.ploudSettings.VerifySignature)
            {
                SignatureRequest signatureRequest = request.ToSignatureRequest(this.ploudSettings.PublicKey, SignatureRequest.Methods.Download);
                SignatureResponse signatureResponse = await signatureRequest.VerifySignatureAsync(this.signatureService);
                
                if (signatureResponse == null || !signatureResponse.IsValid)
                {
                    this.logger.LogWarning("Download.VerifySignature(), Signature = {Signature}", signatureRequest.Signature);
                    
                    return this.Forbid(ValidationStatus.InvalidSignature);
                }
            }

            AuthenticationRequest authenticationRequest = request.ToAuthenticationRequest(this.ploudSettings.PublicKey);
            AuthenticationResponse authenticationResponse = await authenticationRequest.AuthenticateAsync(this.authenticationService, this.memoryCache);
            
            if (authenticationResponse == null || !authenticationResponse.IsAuthenticated)
            {
                this.logger.LogWarning("Download.Authenticate(), Token = {Token}", authenticationRequest.Token);

                return this.Forbid(ValidationStatus.InvalidToken);
            }

            string ploudFilePath = this.ploudSettings.GetPloudFilePath(authenticationResponse.FolderName, authenticationResponse.FileName);
            
            if (!System.IO.File.Exists(ploudFilePath))
            {
                this.logger.LogError("Download.GetPloudFilePath(PloudNotInitialized), PloudFilePath = {PloudFilePath}", ploudFilePath);
                
                return this.BadRequest(ValidationStatus.PloudNotInitialized);
            }

            string ploudDirectory = this.ploudSettings.GetPloudDirectory(authenticationResponse.FolderName);
            SyncSettings syncSettings = new SyncSettings(ploudDirectory, ploudFilePath);
            string cellarFilePath = await request.PrepareForDownloadAsync(this.syncService, syncSettings);
            
            if (string.IsNullOrEmpty(cellarFilePath))
            {
                return this.InternalServerError(ValidationStatus.ServerError);
            }
            
            Stream fileStream = new FileStream(cellarFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.SequentialScan | FileOptions.DeleteOnClose);
            {
                return this.File(fileStream, "application/octet-stream", "PLOC.co");
            }
        }
    }
}
