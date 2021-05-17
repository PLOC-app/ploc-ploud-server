using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public interface ICellar
    {
        ICryptoProvider CryptoProvider { get; }

        ICellarRepository Repository { get; }

        String DatabasePath { get; }

        T CreateObject<T>() where T : IPloudObject;

        bool IsValid();

        SyncObjects GetSyncObjects(SyncObjectsOptions options);

        Task<SyncObjects> GetSyncObjectsAsync(SyncObjectsOptions options);
    }
}
