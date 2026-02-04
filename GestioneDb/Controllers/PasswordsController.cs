using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneDb.Data;
using GestioneDb.Models;
using Microsoft.AspNetCore.Authorization;
using GestioneDb.DTOs;
using Security;

namespace GestioneDb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PasswordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("UserId")]
        public async Task<ActionResult<List<Password>>> GetPasswords(int UserId)
            => Ok(await _context.Passwords.FirstOrDefaultAsync(p => p.UserID == UserId));

        [HttpGet("ById/{id}")]
        public async Task<ActionResult<Password>> GetPasswordById(int id)
        {
            var password = await _context.Passwords.FindAsync(id);

            if (password == null)
                return NotFound();

            return (Ok(password));
        }

        [HttpGet("ByApp/{app}")]
        public async Task<ActionResult<Password>> GetPasswordByApp(string app, int UserID)
        {
            var password = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == app && p.UserID == UserID);

            if (password == null)
                return NotFound();

            return (Ok(password));
        }

        [HttpPost]
        public async Task<ActionResult<Password>> CreatePassword(Password NewPassword)
        {
            var PasswordIn = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == NewPassword.AppName);
            
            if (NewPassword == null || PasswordIn != null)
                return BadRequest();

            _context.Passwords.Add(NewPassword);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreatePassword), new { id = NewPassword.CredentialID}, NewPassword);
        }

        [HttpPut("ById/{id}")]
        public async Task<IActionResult> UpdatePasswordById(Password ModifiedPassword, int id)
        {
            if (ModifiedPassword == null)
                return BadRequest();

            var password = await _context.Passwords.FindAsync(id);

            if (password == null) return NotFound();

            password.AppName = ModifiedPassword.AppName;
            password.EncryptedPassword = ModifiedPassword.EncryptedPassword;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("ByApp/{app}")]
        public async Task<IActionResult> UpdatePasswordByApp(Password ModifiedPassword, string app)
        {
            if (ModifiedPassword == null) return BadRequest();

            var password = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == app);

            if (password == null) return NotFound();

            password.AppName = ModifiedPassword.AppName;
            password.EncryptedPassword = ModifiedPassword.EncryptedPassword;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("ById/{id}")]
        public async Task<IActionResult> DeletePasswordById(int id)
        {
            var password = await _context.Passwords.FindAsync(id);

            if (password == null) return NotFound();

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpDelete("ByApp/{app}")]
        public async Task<IActionResult> DeletePasswordByApp(string app)
        {
            var password = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == app);

            if (password == null) return NotFound();

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
