using GestioneDb.Data;
using GestioneDb.DTOs.Users;
using GestioneDb.Middlewares;
using GestioneDb.Models;
using GestioneDb.Services;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
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
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [SkipUserIdExtraction]
        [AllowAnonymous]
        [HttpGet("get/ById/{id}")]
        public async Task<Result<UserResponseDTO>> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            return result;
        }

        [HttpGet("get/ByToken")]
        public async Task<Result<UserResponseDTO>> GetUserByToken()
        {
            int id = (int) HttpContext.Items["UserId"];
            var result = await _userService.GetUserByIdAsync(id);

            return result;
        }

        [SkipUserIdExtraction]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Result<RegisterResponseDTO>> CreateUser([FromBody] RegisterDTO NewUser, [FromServices] JwtService jwt)
        {
            var result = await _userService.CreateUserAsync(NewUser, jwt);

            return result;
        }

        [HttpPut("update")]
        public async Task<Result<bool>> UpdateUser([FromBody] UpdateUserDTO ModifiedUser)
        {
            int id = (int) HttpContext.Items["UserId"];
            var result = await _userService.UpdateUserByIdAsync(id, ModifiedUser);

            return result;
        }

        [HttpDelete("delete")]
        public async Task<Result<bool>> DeleteUserByToken()
        {
            int id = (int) HttpContext.Items["UserId"];
            var result = await _userService.DeleteUserByIdAsync(id);

            return result;
        }

        [SkipUserIdExtraction]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Result<LoginResponseDTO>> Login(LoginDTO Credentials, [FromServices] JwtService jwt)
        {
            var result = await _userService.LoginAsync(Credentials, jwt);

            return result;
        }
    } 
}
