using System;
using System.IO;

namespace Ploc.Ploud.Api
{
    public static class PloudSettingsExtensions
    {
        public static string GetPloudFilePath(this PloudSettings ploudSettings, string folderName, string userFilename)
        {
            string userDirectory = GetPloudDirectory(ploudSettings, folderName);

            return Path.Combine(userDirectory, userFilename);
        }

        public static string GetPloudDirectory(this PloudSettings ploudSettings, string folderName)
        {
            if (string.IsNullOrEmpty(ploudSettings.Directory))
            {
                throw new ArgumentException("PloudSettings.Directory is not set.");
            }

            string userDirectory = Path.Combine(ploudSettings.Directory, folderName);

            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }

            return userDirectory;
        }
    }
}
