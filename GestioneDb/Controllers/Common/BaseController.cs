using GestioneDb.Services.Common;
using Microsoft.AspNetCore.Mvc;


namespace GestioneDb.Controllers.Common
{
    public abstract class BaseController : ControllerBase
    {

        protected IActionResult HandleError(ErrorCode error, string ErrorString)
        {
            var message = ErrorString ?? "Unknown error";

            return error switch
            {
                ErrorCode.NotFound => NotFound(message),
                ErrorCode.Unauthorized => Unauthorized(message),
                ErrorCode.BadRequest => BadRequest(message),
                ErrorCode.Conflict => Conflict(message),
                _ => StatusCode(500, message)
            };
        }
    }
}
