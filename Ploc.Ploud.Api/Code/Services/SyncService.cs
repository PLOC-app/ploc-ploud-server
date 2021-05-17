using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public sealed class SyncService : ISyncService
    {
        private readonly ILogger<ISyncService> logger;

        public SyncService(ILogger<ISyncService> logger)
        {
            this.logger = logger;
        }

        public async Task<SyncResponse> SynchronizeAsync(SyncRequest syncRequest, SyncSettings syncSettings)
        {
            ICellar cellar = new Cellar(syncSettings.PloudFilePath);
            if (!cellar.IsValid())
            {
                return null;
            }

            SyncObjects syncObjects = syncRequest.Objects;

            // 1. Retrieve and apply changes

            // .Copy file into document bytes[] 
            int numberOfDocuments = syncRequest.Objects.GetCount(syncObjects.Documents);
            int numberOfFiles = syncRequest.Files == null ? 0 : syncRequest.Files.Count;
            if(numberOfDocuments != numberOfFiles)
            {
                this.logger.LogError("SyncService.SynchronizeAsync({Token}), {NumberOfDocuments} != {NumberOfFiles}", syncRequest.Token, numberOfDocuments, numberOfFiles);
                return null;
            }
            if(numberOfFiles > 0)
            {
                for(int position = 0; position < numberOfFiles; position++)
                {
                    Document inputDocument = syncObjects.Documents[position];
                    IFormFile inputFile = syncRequest.Files[position];
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        inputFile.CopyTo(memoryStream);
                        inputDocument.Data = memoryStream.ToArray();
                    }
                }
            }

            // .Delete objects
            if((syncObjects.DeletedObjects != null)
                && (syncObjects.DeletedObjects.Count > 0))
            {
                PloudObjectCollection<IPloudObject> ploudObjectsToDelete = new PloudObjectCollection<IPloudObject>();
                foreach(DeletedObject deletedObject in syncObjects.DeletedObjects)
                {
                    IPloudObject ploudObjectToDelete = deletedObject.PloudObject;
                    if(ploudObjectToDelete == null)
                    {
                        continue;
                    }
                    ploudObjectsToDelete.Add(ploudObjectToDelete);
                }
                await cellar.DeleteAsync(ploudObjectsToDelete);
            }

            // .Send changes to the database
            PloudObjectCollection<IPloudObject> ploudObjectsToUpdate = syncObjects.AllObjects();
            if((ploudObjectsToUpdate != null) 
                && (ploudObjectsToUpdate.Count > 0))
            {
                await cellar.SaveAsync(ploudObjectsToUpdate);
            }

            // 2. Send changes since the last sync
            SyncObjectsOptions syncObjectsOptions = new SyncObjectsOptions(syncRequest.LastSyncTime, syncRequest.Device);
            SyncResponse syncResponse = new SyncResponse();
            syncResponse.Objects = await cellar.GetSyncObjectsAsync(syncObjectsOptions);

            // 3. Write the URLS
            syncResponse.Objects.Documents.MapUrl(syncSettings.DownloadFileUrlFormat);

            return syncResponse;
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
