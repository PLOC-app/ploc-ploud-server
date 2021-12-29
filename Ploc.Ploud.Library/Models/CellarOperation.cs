﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public enum CellarOperation
    {
        Compress,

        Encrypt,

        Decrypt,

        Validate,

        ClearTimestamp // Fix Android timestamp 🙄
    }
}
