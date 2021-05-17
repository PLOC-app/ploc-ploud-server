using Microsoft.AspNetCore.Http;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class InitializeRequest : RequestBase
    {
        public IFormFile File { get; set; }

        public override ValidationStatus Validate()
        {
            ValidationStatus validationStatus = base.Validate();
            if(validationStatus != ValidationStatus.Ok)
            {
                return validationStatus;
            }
            if((this.File == null)
                || (this.File.Length < 1024))
            {
                return ValidationStatus.InvalidParams;
            }
            return ValidationStatus.Ok;
        }
    }
}
