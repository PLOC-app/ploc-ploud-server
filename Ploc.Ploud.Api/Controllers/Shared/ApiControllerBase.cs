using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        public new IActionResult Forbid()
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                Status = Config.Error
            });
        }

        public IActionResult Forbid(ValidationStatus validationError)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }

        public new IActionResult BadRequest()
        {
            return StatusCode(StatusCodes.Status400BadRequest, new
            {
                Status = Config.Error
            });
        }

        public IActionResult BadRequest(ValidationStatus validationError)
        {
            return StatusCode(StatusCodes.Status400BadRequest, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }

        public new IActionResult NotFound()
        {
            return StatusCode(StatusCodes.Status404NotFound, new
            {
                Status = Config.Error
            });
        }

        public IActionResult NotFound(ValidationStatus validationError)
        {
            return StatusCode(StatusCodes.Status404NotFound, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }

        public IActionResult InternalServerError()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Status = Config.Error
            });
        }

        public IActionResult InternalServerError(ValidationStatus validationError)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }
    }
}
