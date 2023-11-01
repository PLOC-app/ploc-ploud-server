using Microsoft.Extensions.Caching.Memory;
using Ploc.Ploud.Library;
using System;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public static class RequestBaseExtensions
    {
        public async static Task<AuthenticationResponse> AuthenticateAsync(this AuthenticationRequest request, IAuthenticationService authenticationService, IMemoryCache memoryCache)
        {
            AuthenticationResponse authenticationResponse = memoryCache.Get<AuthenticationResponse>(request.Token);
            if (authenticationResponse == null)
            {
                authenticationResponse = await authenticationService.AuthenticateAsync(request);
                if (authenticationResponse.IsAuthenticated)
                {
                    MemoryCacheEntryOptions cacheExpiryOptions = CreateMemoryCacheEntryOptions();
                    memoryCache.Set<AuthenticationResponse>(request.Token, authenticationResponse, cacheExpiryOptions);
                }
            }
            return authenticationResponse;
        }

        public static async Task<SignatureResponse> VerifySignatureAsync(this SignatureRequest request, ISignatureService signatureService)
        {
            return await signatureService.VerifySignatureAsync(request);
        }

        public static async Task<NotificationResponse> NotifyAsync(this NotificationRequest request, INotificationService notificationService)
        {
            return await notificationService.NotifyAsync(request);
        }

        public static async Task<SyncResponse> SynchronizeAsync(this SyncRequest request, ISyncService syncService, SyncSettings syncSettings)
        {
            return await syncService.SynchronizeAsync(request, syncSettings);
        }

        public static async Task<Document> GetDocumentAsync(this DocumentRequest request, ISyncService syncService, SyncSettings syncSettings)
        {
            return await syncService.GetDocumentAsync(request, syncSettings);
        }

        public static async Task<bool> InitializeAsync(this InitializeRequest request, ISyncService syncService, SyncSettings syncSettings)
        {
            return await syncService.InitializeAsync(request, syncSettings);
        }

        public static async Task<bool> UninitializeAsync(this UninitializeRequest request, ISyncService syncService, SyncSettings syncSettings)
        {
            return await syncService.UninitializeAsync(request, syncSettings);
        }

        public static async Task<bool> EraseDataAsync(this EraseDataRequest request, ISyncService syncService, SyncSettings syncSettings)
        {
            return await syncService.EraseDataAsync(request, syncSettings);
        }

        public static async Task<String> PrepareForDownloadAsync(this DownloadRequest request, ISyncService syncService, SyncSettings syncSettings)
        {
            return await syncService.PrepareForDownloadAsync(request, syncSettings);
        }

        public static async Task<Dashboard> GetDashboardAsync(this RequestBase request, IDashboardService dashboardService, SyncSettings syncSettings)
        {
            return await dashboardService.GetDashboardAsync(request, syncSettings);
        }

        private static MemoryCacheEntryOptions CreateMemoryCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(30)
            };
        }

        public static AuthenticationRequest ToAuthenticationRequest(this RequestBase request, Guid publicKey)
        {
            return new AuthenticationRequest()
            {
                Token = request.Token,
                Device = request.Device,
                PublicKey = publicKey
            };
        }

        public static SignatureRequest ToSignatureRequest(this RequestBase request, Guid publicKey, String method)
        {
            return new SignatureRequest()
            {
                Signature = request.Signature,
                Timestamp = request.Timestamp,
                LastSyncTime = request.LastSyncTime,
                Token = request.Token,
                PublicKey = publicKey,
                Device = request.Device,
                Method = method
            };
        }

        public static SignatureRequest ToSignatureRequest(this RequestBase request, Guid publicKey, String method, String objectId)
        {
            return new SignatureRequest()
            {
                Signature = request.Signature,
                Timestamp = request.Timestamp,
                LastSyncTime = request.LastSyncTime,
                Token = request.Token,
                PublicKey = publicKey,
                Device = request.Device,
                Method = method,
                ObjectId = objectId
            };
        }

        public static NotificationRequest ToNotificationRequest(this RequestBase request, Guid publicKey)
        {
            return new NotificationRequest()
            {
                Token = request.Token,
                Device = request.Device,
                PublicKey = publicKey
            };
        }
    }
}
