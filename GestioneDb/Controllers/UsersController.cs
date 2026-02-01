using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneDb.Data;
using GestioneDb.Models;
using Microsoft.AspNetCore.Authorization;
using GestioneDb.DTOs;
using Security;

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
        public async Task<ActionResult<UserResponseDTO>> CreateUser(RegisterDTO Credentials)
        {
            if (Credentials == null)
                return BadRequest("Dati utente mancanti");

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == Credentials.Username);

            if (existing != null)
                return BadRequest("Username già esistente");

            string HashedPassword, Salt;

            try
            {
                (HashedPassword, Salt) = Hashing.HashPassword(Credentials.Password);
            }
            catch (Exception)
            {
                return StatusCode(500, "Errore interno durante l'hashing della password");
            }

            var NewUser = new User
            {
                Username = Credentials.Username,
                HashedPassword = HashedPassword,
                PasswordSalt = Salt,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            var response = new UserResponseDTO
            {
                UserID = NewUser.UserID,
                Username = NewUser.Username,
                CreatedAt = NewUser.CreatedAt
            };

            return CreatedAtAction(nameof(GetUserByID), new { id = NewUser.UserID }, response);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO ModifiedUser)
        {
            if (ModifiedUser == null)
                return BadRequest("Dati mancanti");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound("Utente non trovato");

            if (!string.IsNullOrEmpty(ModifiedUser.Username))
            {
                var conflict = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == ModifiedUser.Username && u.UserID != id);

                if (conflict != null)
                    return BadRequest("Username già esistente");

                user.Username = ModifiedUser.Username;
            }

            if (!string.IsNullOrEmpty(ModifiedUser.Password))
            {
                try
                {
                    (user.HashedPassword, user.PasswordSalt) = Hashing.HashPassword(ModifiedUser.Password);
                }
                catch
                {
                    return StatusCode(500, "Errore interno durante l'hashing della password");
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Utente aggiornato");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO credentials, [FromServices] JwtService jwt)
        {
            if (credentials == null)
                return BadRequest("Dati mancanti");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == credentials.Username);

            if (user == null)
                return Unauthorized("Credenziali non valide");

            bool ok = Hashing.VerifyPassword(
                credentials.Password,
                user.HashedPassword,
                user.PasswordSalt
            );

            if (!ok)
                return Unauthorized("Credenziali non valide");

            var tokenUser = new ClassesLibrary.User
            {
                UserID = user.UserID,
                Username = user.Username,
                HashedPassword = user.HashedPassword,
                PasswordSalt = user.PasswordSalt,
                CreatedAt = user.CreatedAt
            };

            var token = jwt.GenerateToken(tokenUser);

            return Ok(new { Token = token });
        }
    } 
}
