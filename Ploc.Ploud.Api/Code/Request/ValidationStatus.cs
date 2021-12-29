using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public enum ValidationStatus
    {
        InvalidParams = 0,

        InvalidAccount = 5,

        InvalidToken = 10,

        InvalidTimestamp = 15,

        InvalidSignature = 20,

        PloudNotInitialized = 25,
        
        PloudAlreadyInitialized = 30,

        ServerError = 35,

        Ok = 200
    }
}
