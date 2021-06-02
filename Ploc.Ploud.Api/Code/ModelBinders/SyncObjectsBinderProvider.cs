using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api.Code.ModelBinders
{
    public class SyncObjectsBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(SyncObjects))
            {
                return new SyncObjectsBinder();
            }
            return null;
        }
    }
}
