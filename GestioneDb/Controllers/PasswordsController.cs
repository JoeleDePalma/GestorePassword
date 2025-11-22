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
            return Ok(_context.Passwords.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Password>> GetPasswordByApp(int id)
        {
            var password = await _context.Passwords.FindAsync(id);

            if (password == null)
                return NotFound();

            return (Ok(password));
        }


        [HttpPost]
        public async Task<ActionResult<Password>> CreatePassword(Password NewPassword)
        {
            var PasswordIn = await _context.Passwords.FindAsync(NewPassword.Id);
            if (NewPassword == null && PasswordIn != null)
                return BadRequest();

            _context.Passwords.Add(NewPassword);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreatePassword), new { id = NewPassword.Id}, NewPassword);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePassword(Password ModifiedPassword, int id)
        {
            var password = await _context.Passwords.FindAsync(id);

            if (password == null) return NotFound();

            password.CryptedPassword = ModifiedPassword.CryptedPassword;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassword(int id)
        {
            var password = await _context.Passwords.FindAsync(id);

            if (password == null) return NotFound();

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
