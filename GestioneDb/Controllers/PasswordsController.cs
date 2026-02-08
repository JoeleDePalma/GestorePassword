using GestioneDb.Data;
using GestioneDb.DTOs.Passwords;
using GestioneDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Security;
using ControllersServices;
using System.Security.Cryptography.Xml;

namespace GestioneDb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private int _userId;
        private readonly ApplicationDbContext _context;
        private readonly ControllersServices.ControllersServices _services;

        public PasswordsController(ApplicationDbContext context, ControllersServices.ControllersServices services)
        {
            _context = context;
            _services = services;
        }

        [HttpGet("UserId")]
        public async Task<ActionResult<List<PasswordResponseDTO>>> GetPasswords(string MasterPassword)
        {
            byte[] Key;
            _userId = GetUserId();

            List<Password> PasswordsList = await _context.Passwords
                .Where(p => p.UserID == _userId)
                    .ToListAsync();

            if (!PasswordsList.Any())
                return Ok(new List<PasswordResponseDTO>());

            List<PasswordResponseDTO> InfoList = new List<PasswordResponseDTO>();

            foreach (Password p in PasswordsList)
            {
                (Key, _) = await _services.KeyFromPassword(MasterPassword, _userId, p.KeySalt);

                if (Key == null)
                    return Unauthorized();

                InfoList.Add(new PasswordResponseDTO
                {
                    AppName = p.AppName,
                    AppUsername = p.AppUsername,
                    Password = CryptographyService.Decrypt(p.EncryptedPassword, Key),
                    CreatedAt = p.CreatedAt,
                    LastUpdateAt = p.LastUpdateAt
                });
            }

            return Ok(InfoList);
        }

        [HttpGet("ById/{id}")]
        public async Task<ActionResult<PasswordResponseDTO>> GetPasswordById(int id, string MasterPassword)
        {
            _userId = GetUserId();

            Password? password = await _context.Passwords.FindAsync(id);

            if (password == null)
                return NotFound();

            if (password.UserID != _userId)
                return Unauthorized();

            var (Key, _) = await _services.KeyFromPassword(MasterPassword, _userId, password.KeySalt);

            if (Key == null)
                return Unauthorized();

            PasswordResponseDTO Info = new PasswordResponseDTO()
            {
                AppName = password.AppName,
                AppUsername = password.AppUsername,
                Password = CryptographyService.Decrypt(password.EncryptedPassword, Key),
                CreatedAt = password.CreatedAt,
                LastUpdateAt = password.LastUpdateAt
            };

            return (Ok(Info));
        }

        [HttpGet("ByApp/{app}")]
        public async Task<ActionResult<PasswordResponseDTO>> GetPasswordByApp(string app, string MasterPassword)
        {
            _userId = GetUserId();

            Password? password = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == app && p.UserID == _userId);

            if (password == null)
                return NotFound();

            var (Key, _) = await _services.KeyFromPassword(MasterPassword, _userId, password.KeySalt);

            if (Key == null)
                return Unauthorized();

            PasswordResponseDTO Info = new PasswordResponseDTO()
            {
                AppName = password.AppName,
                AppUsername = password.AppUsername,
                Password = CryptographyService.Decrypt(password.EncryptedPassword, Key),
                CreatedAt = password.CreatedAt,
                LastUpdateAt = password.LastUpdateAt
            };

            return (Ok(Info));
        }

        [HttpPost]
        public async Task<ActionResult<Password>> CreatePassword(UpdatePasswordDTO InfoNewPassword)
        {
            _userId = GetUserId();

            var PasswordIn = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == InfoNewPassword.AppName && p.UserID == _userId);
            
            if (InfoNewPassword == null || PasswordIn != null)
                return BadRequest();

            var (Key, KeySalt) = await _services.KeyFromPassword(InfoNewPassword.MasterPassword, _userId);

            Password NewPassword = new Password()
            {
                UserID = _userId,
                AppName = InfoNewPassword.AppName,
                AppUsername = InfoNewPassword.AppUsername,
                EncryptedPassword = CryptographyService.Encrypt(InfoNewPassword.Password, Key),
                KeySalt = KeySalt,
                CreatedAt = DateTime.UtcNow,
                LastUpdateAt = DateTime.UtcNow
            };

            _context.Passwords.Add(NewPassword);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(CreatePassword), new { id = NewPassword.CredentialID}, NewPassword);
        }

        [HttpPut("ById/{id}")]
        public async Task<IActionResult> UpdatePasswordById(UpdatePasswordDTO ModifiedPassword, int id)
        {
            _userId = GetUserId();

            var OldPassword = await _context.Passwords.FindAsync(id);

            if (OldPassword == null)
                return NotFound();

            if (OldPassword.UserID != _userId)
                return Unauthorized();

            if (ModifiedPassword == null)
                return BadRequest();

            var (Key, _) = await _services.KeyFromPassword(ModifiedPassword.MasterPassword, _userId, OldPassword.KeySalt);

            if (Key == null)
                return Unauthorized();

            var EncryptedPassword = CryptographyService.Encrypt(ModifiedPassword.Password, Key);

            if (ModifiedPassword.AppName != null)
                OldPassword.AppName = ModifiedPassword.AppName;

            if (ModifiedPassword.AppUsername != null)
                OldPassword.AppUsername = ModifiedPassword.AppUsername;

            if(ModifiedPassword.Password != null)
                OldPassword.EncryptedPassword = EncryptedPassword;

            OldPassword.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("ByApp/{app}")]
        public async Task<IActionResult> UpdatePasswordByApp(UpdatePasswordDTO ModifiedPassword)
        {
            _userId = GetUserId();

            var OldPassword = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == ModifiedPassword.AppName && p.UserID == _userId);

            if (OldPassword == null)
                return NotFound();

            if (ModifiedPassword == null) 
                return BadRequest();

            if (string.IsNullOrWhiteSpace(ModifiedPassword.AppName))
                return BadRequest("AppName is required for UpdateByApp.");

            var (Key, _) = await _services.KeyFromPassword(ModifiedPassword.MasterPassword, _userId, OldPassword.KeySalt);

            if (Key == null)
                return Unauthorized();

            var EncryptedPassword = CryptographyService.Encrypt(ModifiedPassword.Password, Key);

            if (ModifiedPassword.AppName != null)
                OldPassword.AppName = ModifiedPassword.AppName;

            if (ModifiedPassword.AppUsername != null)
                OldPassword.AppUsername = ModifiedPassword.AppUsername;

            if (ModifiedPassword.Password != null)
                OldPassword.EncryptedPassword = EncryptedPassword;

            OldPassword.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("ById/{id}")]
        public async Task<IActionResult> DeletePasswordById(int id)
        {
            _userId = GetUserId();

            var password = await _context.Passwords.FindAsync(id);

            if (password == null) 
                return NotFound();

            if (password.UserID != _userId)
                return Unauthorized();
            
            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        [HttpDelete("ByApp/{app}")]
        public async Task<IActionResult> DeletePasswordByApp(string app)
        {
            _userId = GetUserId();

            var password = await _context.Passwords.FirstOrDefaultAsync(p => p.AppName == app && p.UserID == _userId);

            if (password == null) 
                return NotFound();

            _context.Passwords.Remove(password);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub).Value);
        }
    }
}
