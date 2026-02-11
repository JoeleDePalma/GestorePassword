using GestioneDb.DTOs.Passwords;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetAll(string masterPassword)
        {
            int userId = GetUserId();
            var result = await _passwordService.GetAllAsync(userId, masterPassword);

            return Ok(result);
        }

        [HttpGet("ById/{id}")]
        public async Task<IActionResult> GetById(int id, string masterPassword)
        {
            int userId = GetUserId();
            var result = await _passwordService.GetByIdAsync(id, userId, masterPassword);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("ByApp/{app}")]
        public async Task<IActionResult> GetByApp(string app, string masterPassword)
        {
            int userId = GetUserId();
            var result = await _passwordService.GetByAppAsync(app, userId, masterPassword);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(UpdatePasswordDTO dto)
        {
            int userId = GetUserId();
            var created = await _passwordService.CreateAsync(dto, userId);

            if (created == null)
                return BadRequest("Password already exists for this app.");

            return CreatedAtAction(nameof(GetById), new { id = created.CredentialID }, created);
        }

        [HttpPut("ById/{id}")]
        public async Task<IActionResult> UpdateById(int id, UpdatePasswordDTO dto)
        {
            int userId = GetUserId();
            bool success = await _passwordService.UpdateByIdAsync(id, dto, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpPut("ByApp/{app}")]
        public async Task<IActionResult> UpdateByApp(string app, UpdatePasswordDTO dto)
        {
            int userId = GetUserId();

            bool success = await _passwordService.UpdateByAppAsync(app, dto, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("ById/{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            int userId = GetUserId();
            bool success = await _passwordService.DeleteByIdAsync(id, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("ByApp/{app}")]
        public async Task<IActionResult> DeleteByApp(string app)
        {
            int userId = GetUserId();
            bool success = await _passwordService.DeleteByAppAsync(app, userId);

            if (!success)
                return NotFound();

            return NoContent();
        }

        private int GetUserId()
            => int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
        
    }
}