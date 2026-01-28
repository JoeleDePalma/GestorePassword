using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneDb.Data;
using GestioneDb.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestioneDb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("ById/{id}")]
        public async Task<ActionResult<User>> GetUserByID(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            return (Ok(user));
        }

        [HttpGet("ByUsername/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
                return NotFound();

            return (Ok(user));
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User NewUser)
        {
            if (NewUser == null)
                return BadRequest("Dati utente mancanti");

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == NewUser.Username);

            if (existing != null)
                return BadRequest("Username già esistente");


            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUserByID), new {id = NewUser.UserID}, NewUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User modifiedUser)
        {
            if (modifiedUser == null)
                return BadRequest("Dati mancanti");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("Utente non trovato");

            var conflict = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == modifiedUser.Username && u.UserID != id);

            if (conflict != null)
                return BadRequest("Username già esistente");

            user.Username = modifiedUser.Username;
            user.HashedPassword = modifiedUser.HashedPassword;
            user.PasswordSalt = modifiedUser.PasswordSalt;

            await _context.SaveChangesAsync();

            return Ok("Utente aggiornato");
        }
    }
}
