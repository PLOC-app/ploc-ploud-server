using System;
using System.IO;

namespace Ploc.Ploud.Api
{
    public static class PloudSettingsExtensions
    {
        public static String GetPloudFilePath(this PloudSettings ploudSettings, String folderName, String userFilename)
        {
            String userDirectory = GetPloudDirectory(ploudSettings, folderName);
            return Path.Combine(userDirectory, userFilename);
        }

        public static String GetPloudDirectory(this PloudSettings ploudSettings, String folderName)
        {
            if (String.IsNullOrEmpty(ploudSettings.Directory))
            {
                throw new ArgumentException("PloudSettings.Directory is not set.");
            }
            String userDirectory = Path.Combine(ploudSettings.Directory, folderName);
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }
            return userDirectory;
        }
    }
}
