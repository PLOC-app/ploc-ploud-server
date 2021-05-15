using System;
using System.Collections.Generic;

namespace Ploc.Ploud.Library
{
    public interface ICellar
    {
        ICryptoProvider CryptoProvider { get; }

        ICellarRepository Repository { get; }

        String DatabasePath { get; }

        T CreateObject<T>() where T : IPloudObject;

        SyncObjects GetSyncObjects(SyncObjectsOptions options);
    }
}
