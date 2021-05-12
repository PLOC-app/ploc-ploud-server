using System;
using System.Collections.Generic;
using System.IO;

namespace Ploc.Ploud.Library
{
    public sealed class Cellar : ICellar
    {
        public Cellar(ICellarRepository repository, ICryptoProvider cryptoProvider, String databasePath)
        {
            if (cryptoProvider == null)
            {
                throw new ArgumentNullException("ICryptoProvider");
            }
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
            this.CryptoProvider = cryptoProvider;
            this.Repository = repository;
            this.DatabasePath = databasePath;
        }

        public Cellar(ICryptoProvider cryptoProvider, String databasePath)
            : this(new SqliteRepository(cryptoProvider, databasePath), cryptoProvider, databasePath)
        {

        }

        public ICryptoProvider CryptoProvider { get; private set; }

        public ICellarRepository Repository { get; private set; }

        public String DatabasePath { get; private set; }
    }
}
