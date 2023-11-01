using Ploc.Ploud.Library;
using System;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public interface ISyncService
    {
        Task<SyncResponse> SynchronizeAsync(SyncRequest syncRequest, SyncSettings syncSettings);

        Task<Document> GetDocumentAsync(DocumentRequest documentRequest, SyncSettings syncSettings);

        Task<String> PrepareForDownloadAsync(DownloadRequest request, SyncSettings syncSettings);

        Task<bool> InitializeAsync(InitializeRequest request, SyncSettings syncSettings);

        Task<bool> UninitializeAsync(UninitializeRequest request, SyncSettings syncSettings);

        Task<bool> EraseDataAsync(EraseDataRequest request, SyncSettings syncSettings);
    }
}
