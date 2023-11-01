using Ploc.Ploud.Library;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace Ploc.Ploud.UnitTests
{
    public static class Shared
    {
        private static ConcurrentDictionary<String, String> databases = new ConcurrentDictionary<string, string>();

        public const String ObjectIdentifier = "hw";

        public const String ObjectName = "Hello World";

        public static void CopyDatabase(String destFilePath)
        {
            if (File.Exists(destFilePath))
            {
                return;
            }
            using (var resource = typeof(Shared).Assembly.GetManifestResourceStream("Ploc.Ploud.UnitTests.Resources.fr.ploc.co"))
            {
                using (var file = new FileStream(destFilePath, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        public static void DeleteDatabase(String key)
        {
            if (!File.Exists(DatabasePath(key)))
            {
                return;
            }
            File.Delete(DatabasePath(key));
        }

        public static ICellar Cellar(String key)
        {
            CopyDatabase(DatabasePath(key));
            return new Cellar(DatabasePath(key));
        }

        public static String DatabasePath(String key)
        {
            if (!databases.ContainsKey(key))
            {
                databases.TryAdd(key, Path.Combine(Path.GetTempPath(), "PLOUD." + key + ".co"));
            }
            return databases[key];
        }
    }
}
