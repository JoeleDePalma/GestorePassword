using GestioneDb.Controllers.Common;
using GestioneDb.Data;
using GestioneDb.DTOs.Users;
using GestioneDb.Models;
using GestioneDb.Services;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GestioneDb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
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

            return Ok(result.Data);
        }
    } 
}
