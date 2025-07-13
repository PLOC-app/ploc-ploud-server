using System;

namespace Ploc.Ploud.Api
{
    public class SyncSettings
    {
        public SyncSettings(string ploudDirectory, string ploudFilePath)
        {
            if (string.IsNullOrEmpty(ploudDirectory))
            {
                throw new ArgumentNullException("PloudDirectory");
            }

            if (string.IsNullOrEmpty(ploudFilePath))
            {
                throw new ArgumentNullException("PloudFilePath");
            }

            this.PloudDirectory = ploudDirectory;
            this.PloudFilePath = ploudFilePath;
        }

        public SyncSettings(string ploudDirectory, string ploudFilePath, string downloadFileUrlFormat)
        {
            if (string.IsNullOrEmpty(ploudDirectory))
            {
                throw new ArgumentNullException("PloudDirectory");
            }

            if (string.IsNullOrEmpty(ploudFilePath))
            {
                throw new ArgumentNullException("PloudFilePath");
            }
            
            if (string.IsNullOrEmpty(downloadFileUrlFormat))
            {
                throw new ArgumentNullException("DownloadFileUrlFormat");
            }
            
            this.PloudDirectory = ploudDirectory;
            this.PloudFilePath = ploudFilePath;
            this.DownloadFileUrlFormat = downloadFileUrlFormat;
        }

        public string PloudFilePath { get; set; }

        public string PloudDirectory { get; set; }

        public string DownloadFileUrlFormat { get; set; }

        public bool IsPloudEnabled
        {
            get
            {
                return System.IO.File.Exists(this.PloudFilePath);
            }
        }
    }
}
