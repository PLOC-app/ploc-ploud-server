using Microsoft.AspNetCore.Http;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class SyncRequest : RequestBase
    {
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
            return ValidationStatus.Ok;
        }
    }
}
