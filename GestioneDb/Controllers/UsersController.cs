using GestioneDb.Data;
using GestioneDb.DTOs.Users;
using GestioneDb.Models;
using GestioneDb.Services;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security;
using System.IdentityModel.Tokens.Jwt;
using GestioneDb.Services.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;

namespace GestioneDb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        
        [AllowAnonymous]
        [HttpGet("get/ById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            if (!result.Success)
                return HandleError(result.Error, result.ErrorString);

            return (Ok(result.Data));
        }

        [HttpGet("get/ByToken")]
        public async Task<IActionResult> GetUserByToken()
        {
            int id = GetUserId();

            var result = await _userService.GetUserByIdAsync(id);

            if (!result.Success)
                return HandleError(result.Error, result.ErrorString);

            return (Ok(result.Data));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDTO NewUser, [FromServices] JwtService jwt)
        {
            var result = await _userService.CreateUserAsync(NewUser, jwt);

            if (!result.Success)
                return HandleError(result.Error, result.ErrorString);

            return CreatedAtAction(nameof(GetUserById), new { id = result.Data.UserID }, result.Data);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO ModifiedUser)
        {
            int id = GetUserId();
            var result = await _userService.UpdateUserByIdAsync(id, ModifiedUser);

            if (!result.Success)
                return HandleError(result.Error, result.ErrorString);

            return Ok(result.Data);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserByToken()
        {
            int id = GetUserId();
            var result = await _userService.DeleteUserByIdAsync(id);

            if (!result.Success)
                return HandleError(result.Error, result.ErrorString);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO Credentials, [FromServices] JwtService jwt)
        {
            var result = await _userService.LoginAsync(Credentials, jwt);

            if (!result.Success)
                return HandleError(result.Error, result.ErrorString);

            return Ok(new { token = result.Data.Token });
        }

        private int GetUserId()
            => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        private IActionResult HandleError(ErrorCode error, string ErrorString)
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
