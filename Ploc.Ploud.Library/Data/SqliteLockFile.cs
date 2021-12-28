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

        public bool Lock()
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
                    return false;
                }
            }
            File.WriteAllText(this.LockFilePath, "*");
            return true;
        }

        public void Unlock()
        {
            int retryCount = 0;
            while (true)
            {
                if (!File.Exists(this.LockFilePath))
                {
                    break;
                }
                try
                {
                    File.Delete(this.LockFilePath);
                    break;
                } 
                catch
                {
                    Thread.Sleep(Config.Data.RetryDelay);
                    if (++retryCount > Config.Data.MaxRetries)
                    {
                        return;
                    }
                }
            }
        }
    }
}
