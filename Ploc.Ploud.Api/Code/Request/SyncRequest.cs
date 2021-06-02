using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ploc.Ploud.Api.Code.ModelBinders;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class SyncRequest : RequestBase
    {
        [ModelBinder(BinderType = typeof(SyncObjectsBinder))]
        public SyncObjects Objects { get; set; }

        public List<IFormFile> Files { get; set; }

        public override ValidationStatus Validate()
        {
            ValidationStatus validationStatus = base.Validate();
            if(validationStatus != ValidationStatus.Ok)
            {
                return validationStatus;
            }
            if(Objects == null)
            {
                return ValidationStatus.InvalidParams;
            }
            if(!ValidateFiles())
            {
                return ValidationStatus.InvalidParams;
            }
            return ValidationStatus.Ok;
        }

        private bool ValidateFiles()
        {
            if((this.Files != null)
                && (this.Files.Count > 0))
            {
                foreach(IFormFile formFile in this.Files)
                {
                    if(formFile.Length == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
