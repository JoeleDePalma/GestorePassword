using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestioneDb.Data;
using GestioneDb.Models;
using Microsoft.AspNetCore.Authorization;
using Security;
using GestioneDb.DTOs.Users;

namespace GestioneDb.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<UserResponseDTO>> GetUserByID(int id)
        {
            var Response = await _context.Users.FindAsync(id);

            if (Response == null)
                return NotFound();

            var user = new UserResponseDTO()
            {
                UserID = Response.UserID,
                Username = Response.Username,
                CreatedAt = Response.CreatedAt 
            };

            return (Ok(user));
        }

        [HttpGet("ByUsername/{username}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserByUsername(string username)
        {
            var Response = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);

            if (Response == null)
                return NotFound();

            var user = new UserResponseDTO()
            {
                UserID = Response.UserID,
                Username = Response.Username,
                CreatedAt = Response.CreatedAt
            };

            return (Ok(user));
        }

        [AllowAnonymous]
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
                (HashedPassword, Salt) = HashingService.HashPassword(Credentials.Password);
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
                    (user.HashedPassword, user.PasswordSalt) = HashingService.HashPassword(ModifiedUser.Password);
                }
                catch
                {
                    return StatusCode(500, "Errore interno durante l'hashing della password");
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Utente aggiornato");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginDTO credentials, [FromServices] JwtService jwt)
        {
            if (credentials == null)
                return BadRequest("Dati mancanti");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == credentials.Username);

            if (user == null)
                return Unauthorized("Credenziali non valide");

            bool ok = HashingService.VerifyPassword(
                credentials.Password,
                user.HashedPassword,
                user.PasswordSalt
            );

            if (!ok)
                return Unauthorized("Credenziali non valide");

            var tokenUser = new User
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
