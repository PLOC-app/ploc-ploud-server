using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public sealed class SyncService : ISyncService
    {
        public async Task<SyncResponse> SynchronizeAsync(SyncRequest syncRequest, SyncSettings syncSettings)
        {
            ICellar cellar = new Cellar(syncSettings.PloudFilePath);
            if (!cellar.IsValid())
            {
                return null;
            }

            // 1. Récupérer et appliquer les changements


            // 2. Envoyer les changements depuis la dernière synchro
            SyncObjectsOptions syncObjectsOptions = new SyncObjectsOptions(syncRequest.LastSyncTime, syncRequest.Device);
            SyncResponse syncResponse = new SyncResponse();
            syncResponse.Objects = await cellar.GetSyncObjectsAsync(syncObjectsOptions);

            // 3. Réecrire si besoin les URLS des documents
            syncResponse.Objects.Documents.MapUrl(syncSettings.DownloadFileUrlFormat);


            return null;
        }

        public async Task<Document> GetDocumentAsync(DocumentRequest documentRequest, SyncSettings syncSettings)
        {
            ICellar cellar = new Cellar(syncSettings.PloudFilePath);
            if(!cellar.IsValid())
            {
                return null;
            }
            return await cellar.GetAsync<Document>(documentRequest.Document);
        }

        public async Task<bool> InitializeAsync(InitializeRequest request, SyncSettings syncSettings)
        {
            if(request.File == null)
            {
                return false;
            }

            if(!Directory.Exists(syncSettings.PloudDirectory))
            {
                Directory.CreateDirectory(syncSettings.PloudDirectory);
            }

            // Delete all files
            this.Delete(syncSettings.PloudDirectory);
            this.Delete(syncSettings.PloudFilePath);

            // Save file
            using (Stream fileStream = new FileStream(syncSettings.PloudFilePath, FileMode.Create))
            {
                await request.File.CopyToAsync(fileStream);
            }

            ICellar cellar = new Cellar(syncSettings.PloudFilePath);
            return cellar.IsValid();
        }

        public async Task<bool> UninitializeAsync(UninitializeRequest request, SyncSettings syncSettings)
        {
            // Delete all files
            await Task.Run(() =>
            {
                this.Delete(syncSettings.PloudDirectory);
                this.Delete(syncSettings.PloudFilePath);
            });
            return true;
        }

        private void Delete(String fileOrDirectory)
        {
            FileAttributes fileAttributes = File.GetAttributes(fileOrDirectory);
            if (fileAttributes.HasFlag(FileAttributes.Directory))
            {
                foreach (String filePath in Directory.GetFiles(fileOrDirectory))
                {
                    File.Delete(filePath);
                }
            }
            else
            {
                if (File.Exists(fileOrDirectory))
                {
                    File.Delete(fileOrDirectory);
                }
            }
        }
    }
}
