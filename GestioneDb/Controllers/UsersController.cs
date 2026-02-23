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

        [HttpGet("get/ById/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            if (!result.Success)
                return HandleError(result.Error);

            return (Ok(result.Data));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDTO NewUser, [FromServices] JwtService jwt)
        {
            var result = await _userService.CreateUserAsync(NewUser, jwt);

            if (!result.Success)
                return HandleError(result.Error);

            return CreatedAtAction(nameof(GetUserById), new { id = result.Data.UserID }, result.Data);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO ModifiedUser)
        {
            int id = GetUserId();
            var result = await _userService.UpdateUserByIdAsync(id, ModifiedUser);

            if (!result.Success)
                return HandleError(result.Error);

            return Ok(result.Data);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteUserById()
        {
            int id = GetUserId();
            var result = await _userService.DeleteUserByIdAsync(id);

            if (!result.Success)
                return HandleError(result.Error);

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO Credentials, [FromServices] JwtService jwt)
        {
            var result = await _userService.LoginAsync(Credentials, jwt);

            if (!result.Success)
                return HandleError(result.Error);

            return Ok(new { Token = result.Data });
        }

        private int GetUserId()
            => int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);

        private IActionResult HandleError(ErrorCode error)
        {
            return error switch
            {
                ErrorCode.NotFound => NotFound(),
                ErrorCode.Unauthorized => Unauthorized(),
                ErrorCode.BadRequest => BadRequest(),
                ErrorCode.Conflict => Conflict(),
                _ => StatusCode(500)
            };
        }
    } 
}
