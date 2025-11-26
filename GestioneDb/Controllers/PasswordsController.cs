using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneDb.Data;
using GestioneDb.Models;

namespace GestioneDb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PasswordsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Password>>> GetPasswords()
        {
            return Ok(await _context.Passwords.ToListAsync());
        }

        [HttpGet("ById/{id}")]
        public async Task<ActionResult<Password>> GetPasswordById(int id)
        {
            var password = await _context.Passwords.FindAsync(id);

            if (password == null)
                return NotFound();

            return (Ok(password));
        }

        [HttpGet("ByApp/{app}")]
        public async Task<ActionResult<Password>> GetPasswordByApp(string app)
        {
            var password = await _context.Passwords.FirstOrDefaultAsync(p => p.App == app);

            if (password == null)
                return NotFound();

            return (Ok(password));
        }

        [HttpPost]
        public async Task<ActionResult<Password>> CreatePassword(Password NewPassword)
        {
            var PasswordIn = await _context.Passwords.FirstOrDefaultAsync(p => p.App == NewPassword.App);
            
            if (NewPassword == null || PasswordIn != null)
                return BadRequest();

            _context.Passwords.Add(NewPassword);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreatePassword), new { id = NewPassword.Id}, NewPassword);
        }

        [HttpPut("ById/{id}")]
        public async Task<IActionResult> UpdatePasswordById(Password ModifiedPassword, int id)
        {
            if (ModifiedPassword == null)
                return BadRequest();

            var password = await _context.Passwords.FindAsync(id);

            if (password == null) return NotFound();

            password.App = ModifiedPassword.App;
            password.CryptedPassword = ModifiedPassword.CryptedPassword;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("ByApp/{app}")]
        public async Task<IActionResult> UpdatePasswordByApp(Password ModifiedPassword, string app)
        {
            if (ModifiedPassword == null) return BadRequest();

            var password = await _context.Passwords.FirstOrDefaultAsync(p => p.App == app);

            if (password == null) return NotFound();

            password.App = ModifiedPassword.App;
            password.CryptedPassword = ModifiedPassword.CryptedPassword;

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
            var password = await _context.Passwords.FirstOrDefaultAsync(p => p.App == app);

            if (password == null) return NotFound();

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
