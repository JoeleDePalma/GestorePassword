using GestioneDb.DTOs.Passwords;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

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

        [HttpGet]
        public async Task<IActionResult> GetAllPasswords(string masterPassword)
        {
            int userId = GetUserId();
            var result = await _passwordService.GetAllPasswordsAsync(userId, masterPassword);

            if (!result.Success)
                return HandleError(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetPasswordById(int id, string masterPassword)
        {
            int userId = GetUserId();
            var result = await _passwordService.GetPasswordByIdAsync(id, userId, masterPassword);

            if (!result.Success)
                return HandleError(result.Error);

            return Ok(result.Data);
        }

        [HttpGet("ByApp/{app}")]
        public async Task<IActionResult> GetPasswordByApp(string app, string masterPassword)
        {
            int userId = GetUserId();
            var result = await _passwordService.GetPasswordByAppAsync(app, userId, masterPassword);

            if (!result.Success)
                return HandleError(result.Error);

            return Ok(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePassword(UpdatePasswordDTO dto)
        {
            int userId = GetUserId();
            var result = await _passwordService.CreatePasswordAsync(dto, userId);

            if (!result.Success)
                return HandleError(result.Error);

            return CreatedAtAction(nameof(GetPasswordById), new { id = result.Data.CredentialID }, result);
        }

        [HttpPut("ById/{id}")]
        public async Task<IActionResult> UpdatePasswordById(int id, UpdatePasswordDTO dto)
        {
            int userId = GetUserId();
            var result = await _passwordService.UpdatePasswordByIdAsync(id, dto, userId);

            if (!result.Success)
                return HandleError(result.Error);

            return NoContent();
        }

        [HttpPut("ByApp/{app}")]
        public async Task<IActionResult> UpdatePasswordByApp(string app, UpdatePasswordDTO dto)
        {
            int userId = GetUserId();
            var result = await _passwordService.UpdatePasswordByAppAsync(app, dto, userId);

            if (!result.Success)
                return HandleError(result.Error);

            return NoContent();
        }

        [HttpDelete("ById/{id}")]
        public async Task<IActionResult> DeletePasswordById(int id)
        {
            int userId = GetUserId();
            var result = await _passwordService.DeletePasswordByIdAsync(id, userId);

            if (!result.Success)
                return HandleError(result.Error);

            return NoContent();
        }

        [HttpDelete("ByApp/{app}")]
        public async Task<IActionResult> DeletePasswordByApp(string app)
        {
            int userId = GetUserId();
            var result = await _passwordService.DeletePasswordByAppAsync(app, userId);

            if (!result.Success)
                return HandleError(result.Error);

            return NoContent();
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