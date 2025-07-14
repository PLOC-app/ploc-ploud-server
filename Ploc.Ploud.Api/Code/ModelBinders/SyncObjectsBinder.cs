using Microsoft.AspNetCore.Mvc.ModelBinding;
using Ploc.Ploud.Library;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api.Code.ModelBinders
{
    public class SyncObjectsBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            string rawSyncObjects = bindingContext.HttpContext.Request.Form["Objects"]; // TODO GetName from ?

            if (string.IsNullOrEmpty(rawSyncObjects))
            {
                return Task.CompletedTask;
            }

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            SyncObjects syncObjects = JsonSerializer.Deserialize<SyncObjects>(rawSyncObjects, jsonSerializerOptions);
            
            if (syncObjects == null)
            {
                return Task.CompletedTask;
            }
            
            bindingContext.Result = ModelBindingResult.Success(syncObjects);

            return Task.CompletedTask;
        }
    }
}
