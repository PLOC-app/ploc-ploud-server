using System;
using System.Collections.Generic;
using System.IO;

namespace Ploc.Ploud.Library
{
    public sealed class Cellar : ICellar
    {
        public Cellar(ICellarRepository repository, String databasePath)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("Repository");
            }
            if (String.IsNullOrEmpty(databasePath))
            {
                throw new ArgumentNullException("DatabasePath");
            }
            if (!File.Exists(databasePath))
            {
                throw new FileNotFoundException(databasePath);
            }
            this.Repository = repository;
            this.DatabasePath = databasePath;
            this.InitializeCryptoProvider();
        }

        public Cellar(String databasePath)
        {
            if (String.IsNullOrEmpty(databasePath))
            {
                throw new ArgumentNullException("DatabasePath");
            }
            if (!File.Exists(databasePath))
            {
                throw new FileNotFoundException(databasePath);
            }
            this.Repository = new SqliteCellarRepository(this, databasePath);
            this.DatabasePath = databasePath;
            this.InitializeCryptoProvider();
        }

        public ICryptoProvider CryptoProvider { get; private set; }

        public ICellarRepository Repository { get; private set; }

        public String DatabasePath { get; private set; }

        public T CreateObject<T>() where T : IPloudObject
        {
            T obj = Activator.CreateInstance<T>();
            obj.Cellar = this;
            return obj;
        }

        private void InitializeCryptoProvider()
        {
            this.Repository.CreateStorage<PloudSecret>();
            PloudSecret ploudSecret = this.Repository.Get<PloudSecret>(PloudSecret.GlobalIdentifier);
            if(ploudSecret == null)
            {
                AesCryptoProvider aesCryptoProvider = new AesCryptoProvider();
                ploudSecret = this.CreateObject<PloudSecret>(); 
                ploudSecret.Key = aesCryptoProvider.EncryptedKey;
                ploudSecret.Iv = aesCryptoProvider.EncryptedIv;
                ploudSecret.Version = Config.Version;
                ploudSecret.Save();
                this.CryptoProvider = aesCryptoProvider;
                this.Repository.Execute(CellarOperation.Encrypt);
            }
            else
            {
                this.CryptoProvider = new AesCryptoProvider(ploudSecret.Key, ploudSecret.Iv);
            }
        }
    }
}
