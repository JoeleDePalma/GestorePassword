using GestioneDb.DTOs.Passwords;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GestioneDb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordsController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        [HttpGet("get/all")]
        public async Task<Result<List<PasswordResponseDTO>>> GetAllPasswords(string masterPassword)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.GetAllPasswordsAsync(userId, masterPassword);

            return result;
        }

        [HttpGet("get/ById/{id}")]
        public async Task<Result<PasswordResponseDTO>> GetPasswordById(int id, string masterPassword)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.GetPasswordByIdAsync(id, userId, masterPassword);

            return result;
        }

        [HttpGet("get/ByApp/{app}")]
        public async Task<Result<PasswordResponseDTO>> GetPasswordByApp(string app, string masterPassword)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.GetPasswordByAppAsync(app, userId, masterPassword);

            return result;
        }

        [HttpPost("create")]
        public async Task<Result<CreatedPasswordDTO>> CreatePassword(CreatePasswordDTO NewPassword)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.CreatePasswordAsync(NewPassword, userId);

            return result;
        }

        [HttpPut("update/ById/{id}")]
        public async Task<Result<bool>> UpdatePasswordById(int id, UpdatePasswordDTO dto)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.UpdatePasswordByIdAsync(id, dto, userId);

            return result;
        }

        [HttpPut("update/ByApp/{app}")]
        public async Task<Result<bool>> UpdatePasswordByApp(string app, UpdatePasswordDTO dto)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.UpdatePasswordByAppAsync(app, dto, userId);

            return result;
        }

        [HttpDelete("delete/ById/{id}")]
        public async Task<Result<bool>> DeletePasswordById(int id)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.DeletePasswordByIdAsync(id, userId);

            return result;
        }

        [HttpDelete("delete/ByApp/{app}")]
        public async Task<Result<bool>> DeletePasswordByApp(string app)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.DeletePasswordByAppAsync(app, userId);

            return result;
        }
    }
}