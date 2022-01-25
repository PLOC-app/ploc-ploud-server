﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public sealed class DashboardService : IDashboardService
    {
        public async Task<Dashboard> GetDashboardAsync(RequestBase request, SyncSettings syncSettings)
        {
            ICellar cellar = new Cellar(syncSettings.PloudFilePath);
            if (!cellar.IsValid())
            {
                return null;
            }
            return await cellar.GetDashboardAsync();
        }
    }
}
