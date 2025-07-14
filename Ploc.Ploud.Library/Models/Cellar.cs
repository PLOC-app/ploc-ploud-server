using System;
using System.IO;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public sealed class Cellar : ICellar
    {
        public ICryptoProvider CryptoProvider { get; private set; }

        public ICellarRepository Repository { get; private set; }

        public string DatabasePath { get; private set; }

        public Cellar(ICellarRepository repository, string databasePath)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("Repository");
            }

            if (string.IsNullOrEmpty(databasePath))
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

        public Cellar(string databasePath)
        {
            if (string.IsNullOrEmpty(databasePath))
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

        public T CreateObject<T>() where T : IPloudObject
        {
            T obj = Activator.CreateInstance<T>();
            obj.Cellar = this;

            return obj;
        }

        public IPloudObject CreateObject(Type ploudObjectType)
        {
            IPloudObject obj = Activator.CreateInstance(ploudObjectType) as IPloudObject;

            if (obj == null)
            {
                throw new ArgumentException(string.Concat(ploudObjectType.ToString(), " != IPloudObject"));
            }

            obj.Cellar = this;

            return obj;
        }

        private void InitializeCryptoProvider()
        {
            this.Repository.CreateStorage<PloudSecret>();
            
            PloudSecret ploudSecret = this.Repository.Get<PloudSecret>(PloudSecret.GlobalIdentifier);

            if (ploudSecret == null)
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

        public bool IsValid()
        {
            if (!File.Exists(this.DatabasePath))
            {
                return false;
            }

            return this.Repository.Execute(CellarOperation.Validate);
        }

        public async Task<SyncObjects> GetSyncObjectsAsync(SyncObjectsOptions options)
        {
            IQuery ploudObjectsQuery = new Query.Builder()
                .AddFilter("tp", ExpressionType.GreaterThanOrEqual, options.Timestamp)
                .AddFilter("sid", ExpressionType.NotEqual, options.CallerId)
                .Build();

            IQuery deletedObjectsQuery = new Query.Builder()
                .AddFilter("tp", ExpressionType.GreaterThanOrEqual, options.Timestamp)
                .AddFilter("sid", ExpressionType.NotEqual, options.CallerId)
                .Build();

            SyncObjects syncObjects = new SyncObjects();
            syncObjects.Countries = await this.Repository.GetAllAsync<Country>(ploudObjectsQuery);
            syncObjects.Regions = await this.Repository.GetAllAsync<Region>(ploudObjectsQuery);
            syncObjects.Appellations = await this.Repository.GetAllAsync<Appellation>(ploudObjectsQuery);
            syncObjects.Classifications = await this.Repository.GetAllAsync<Classification>(ploudObjectsQuery);
            syncObjects.Colors = await this.Repository.GetAllAsync<Color>(ploudObjectsQuery);
            syncObjects.Grapes = await this.Repository.GetAllAsync<Grapes>(ploudObjectsQuery);
            syncObjects.Vendors = await this.Repository.GetAllAsync<Vendor>(ploudObjectsQuery);
            syncObjects.Owners = await this.Repository.GetAllAsync<Owner>(ploudObjectsQuery);
            syncObjects.GlobalParameters = await this.Repository.GetAllAsync<GlobalParameter>(ploudObjectsQuery);
            syncObjects.Wines = await this.Repository.GetAllAsync<Wine>(ploudObjectsQuery);
            syncObjects.TastingNotes = await this.Repository.GetAllAsync<TastingNotes>(ploudObjectsQuery);
            syncObjects.Racks = await this.Repository.GetAllAsync<Rack>(ploudObjectsQuery);
            syncObjects.RackItems = await this.Repository.GetAllAsync<RackItem>(ploudObjectsQuery);
            syncObjects.Documents = await this.Repository.GetAllAsync<Document>(ploudObjectsQuery);
            syncObjects.BottleFormats = this.Repository.GetAll<BottleFormat>(ploudObjectsQuery);
            syncObjects.Orders = await this.Repository.GetAllAsync<Order>(ploudObjectsQuery);
            syncObjects.DeletedObjects = await this.Repository.GetAllAsync<DeletedObject>(deletedObjectsQuery);
            
            syncObjects.RemoveEmptyCollection();

            return syncObjects;
        }

        public SyncObjects GetSyncObjects(SyncObjectsOptions options)
        {
            IQuery ploudObjectsQuery = new Query.Builder()
                .AddFilter("tp", ExpressionType.GreaterThanOrEqual, options.Timestamp)
                .AddFilter("sid", ExpressionType.NotEqual, options.CallerId)
                .Build();

            IQuery deletedObjectsQuery = new Query.Builder()
                .AddFilter("tp", ExpressionType.GreaterThanOrEqual, options.Timestamp)
                .AddFilter("sid", ExpressionType.NotEqual, options.CallerId)
                .Build();

            SyncObjects syncObjects = new SyncObjects();
            syncObjects.Countries = this.Repository.GetAll<Country>(ploudObjectsQuery);
            syncObjects.Regions = this.Repository.GetAll<Region>(ploudObjectsQuery);
            syncObjects.Appellations = this.Repository.GetAll<Appellation>(ploudObjectsQuery);
            syncObjects.Classifications = this.Repository.GetAll<Classification>(ploudObjectsQuery);
            syncObjects.Colors = this.Repository.GetAll<Color>(ploudObjectsQuery);
            syncObjects.Grapes = this.Repository.GetAll<Grapes>(ploudObjectsQuery);
            syncObjects.Vendors = this.Repository.GetAll<Vendor>(ploudObjectsQuery);
            syncObjects.Owners = this.Repository.GetAll<Owner>(ploudObjectsQuery);
            syncObjects.GlobalParameters = this.Repository.GetAll<GlobalParameter>(ploudObjectsQuery);
            syncObjects.Wines = this.Repository.GetAll<Wine>(ploudObjectsQuery);
            syncObjects.TastingNotes = this.Repository.GetAll<TastingNotes>(ploudObjectsQuery);
            syncObjects.Racks = this.Repository.GetAll<Rack>(ploudObjectsQuery);
            syncObjects.RackItems = this.Repository.GetAll<RackItem>(ploudObjectsQuery);
            syncObjects.Documents = this.Repository.GetAll<Document>(ploudObjectsQuery);
            syncObjects.BottleFormats = this.Repository.GetAll<BottleFormat>(ploudObjectsQuery);
            syncObjects.Orders = this.Repository.GetAll<Order>(ploudObjectsQuery);
            syncObjects.DeletedObjects = this.Repository.GetAll<DeletedObject>(deletedObjectsQuery);
            
            syncObjects.RemoveEmptyCollection();

            return syncObjects;
        }
    }
}
