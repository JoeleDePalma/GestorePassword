using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace GestioneDb.Controllers.Common
{
    public abstract class BaseController : ControllerBase
    {
        protected int GetUserId()
            => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

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
