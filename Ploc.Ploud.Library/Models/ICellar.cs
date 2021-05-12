using System;
using System.Collections.Generic;

namespace Ploc.Ploud.Library
{
    public interface ICellar
    {
        ICryptoProvider CryptoProvider { get; }

        ICellarRepository Repository { get; }

        String DatabasePath { get; }
    }
}
