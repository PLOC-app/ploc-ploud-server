using Microsoft.AspNetCore.Http;
using System;

namespace Ploc.Ploud.Api
{
    public class InitializeRequest : RequestBase
    {
        public IFormFile File { get; set; }

        public override ValidationStatus Validate()
        {
            ValidationStatus validationStatus = base.Validate();
            if (validationStatus != ValidationStatus.Ok)
            {
                return validationStatus;
            }
            if ((this.File == null)
                || (this.File.Length < 1024))
            {
                return ValidationStatus.InvalidParams;
            }
            return ValidationStatus.Ok;
        }

        internal static InitializeRequest FromHeaders(HttpRequest request)
        {
            if (!request.ContentLength.HasValue)
            {
                return null;
            }

            if (!request.Headers.ContainsKey("token"))
            {
                return null;
            }
            
            request.EnableBuffering();
            
            InitializeRequest initializeRequest = new InitializeRequest();
            initializeRequest.Token = request.Headers["token"];
            initializeRequest.Device = request.Headers["device"];
            initializeRequest.Signature = request.Headers["signature"];
            initializeRequest.Timestamp = Convert.ToInt64(request.Headers["timestamp"]);
            initializeRequest.LastSyncTime = Convert.ToInt64(request.Headers["lastSyncTime"]);
            initializeRequest.File = new FormFile(request.Body, 0, request.ContentLength.Value, null, "PLOC.co")
            {
                Headers = new HeaderDictionary(),
                ContentType = request.ContentType
            };

            return initializeRequest;
        }
    }
}
