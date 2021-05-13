using Ploc.Ploud.Library;
using System;
using System.IO;

namespace Ploc.Ploud.UnitTests
{
    public static class Shared
    {
        private static String databasePath;

        static Shared()
        {
            databasePath = Path.Combine(Path.GetTempPath(), "PLOUD.co");
            Console.WriteLine("DatabasePath = {0}", databasePath);
        }

        public static void CopyDatabase()
        {
            if(File.Exists(DatabasePath))
            {
                return;
            }
            using (var resource = typeof(Shared).Assembly.GetManifestResourceStream("Ploc.Ploud.UnitTests.Resources.fr.ploc.co"))
            {
                using (var file = new FileStream(DatabasePath, FileMode.Create, FileAccess.Write))
                {
                    resource.CopyTo(file);
                }
            }
        }

        public static void DeleteDatabase()
        {
            if (!File.Exists(DatabasePath))
            {
                return;
            }
            File.Delete(DatabasePath);
        }

        public static ICellar Cellar()
        {
            CopyDatabase();
            return new Cellar(DatabasePath);
        }

        public static String DatabasePath 
        { 
            get
            {
                return databasePath;
            } 
        }
    }
}
