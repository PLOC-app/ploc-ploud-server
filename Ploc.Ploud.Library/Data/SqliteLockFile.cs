using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Ploc.Ploud.Library
{
    public class SqliteLockFile
    {

        public SqliteLockFile(String lockFilePath)
        {
            this.LockFilePath = lockFilePath;
        }

        public String LockFilePath { get; private set; }

        public void Lock()
        {
            int retryCount = 0;
            while(true)
            {
                if(!File.Exists(this.LockFilePath))
                {
                    break;
                }
                Thread.Sleep(Config.Data.RetryDelay);
                if (++retryCount > Config.Data.MaxRetries)
                {
                    break;
                }
            }
            File.WriteAllText(this.LockFilePath, "*");
        }

        public void Unlock()
        {
            if (File.Exists(this.LockFilePath))
            {
                File.Delete(this.LockFilePath);
            }
        }
    }
}
