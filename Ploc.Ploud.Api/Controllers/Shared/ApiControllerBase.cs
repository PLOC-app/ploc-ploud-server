using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ploc.Ploud.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected new IActionResult Forbid()
        {
            return this.StatusCode(StatusCodes.Status403Forbidden, new
            {
                Status = Config.Error
            });
        }

        protected IActionResult Forbid(ValidationStatus validationError)
        {
            return this.StatusCode(StatusCodes.Status403Forbidden, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }

        protected new IActionResult BadRequest()
        {
            return this.StatusCode(StatusCodes.Status400BadRequest, new
            {
                Status = Config.Error
            });
        }

        protected IActionResult BadRequest(ValidationStatus validationError)
        {
            return this.StatusCode(StatusCodes.Status400BadRequest, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }

        protected new IActionResult NotFound()
        {
            return this.StatusCode(StatusCodes.Status404NotFound, new
            {
                Status = Config.Error
            });
        }

        protected IActionResult NotFound(ValidationStatus validationError)
        {
            return this.StatusCode(StatusCodes.Status404NotFound, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }

        protected IActionResult InternalServerError()
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Status = Config.Error
            });
        }

        protected IActionResult InternalServerError(ValidationStatus validationError)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Status = Config.Error,
                Error = validationError
            });
        }
    }
}
