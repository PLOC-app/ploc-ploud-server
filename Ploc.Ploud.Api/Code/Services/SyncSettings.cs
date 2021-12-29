using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class SyncSettings
    {
        public SyncSettings(String ploudDirectory, String ploudFilePath)
        {
            if (String.IsNullOrEmpty(ploudDirectory))
            {
                throw new ArgumentNullException("PloudDirectory");
            }
            if (String.IsNullOrEmpty(ploudFilePath))
            {
                throw new ArgumentNullException("PloudFilePath");
            }
            this.PloudDirectory = ploudDirectory;
            this.PloudFilePath = ploudFilePath;
        }

        public SyncSettings(String ploudDirectory, String ploudFilePath, String downloadFileUrlFormat)
        {
            if (String.IsNullOrEmpty(ploudDirectory))
            {
                throw new ArgumentNullException("PloudDirectory");
            }
            if (String.IsNullOrEmpty(ploudFilePath))
            {
                throw new ArgumentNullException("PloudFilePath");
            }
            if (String.IsNullOrEmpty(downloadFileUrlFormat))
            {
                throw new ArgumentNullException("DownloadFileUrlFormat");
            }
            this.PloudDirectory = ploudDirectory;
            this.PloudFilePath = ploudFilePath;
            this.DownloadFileUrlFormat = downloadFileUrlFormat;
        }

        public String PloudFilePath { get; set; }

        public String PloudDirectory { get; set; }

        public String DownloadFileUrlFormat { get; set; }

        public bool IsPloudEnabled
        {
            get
            {
                return System.IO.File.Exists(this.PloudFilePath);
            }
        }
    }
}
