using GestioneDb.Data;
using GestioneDb.DTOs.Passwords;
using GestioneDb.Models;
using GestioneDb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Security;

namespace GestioneDb.Services.Implementations
{
    public class PasswordService : IPasswordService
    {
        private readonly ApplicationDbContext _context;
        private readonly ControllersServices.ControllersServices _services;

        public PasswordService(ApplicationDbContext context, ControllersServices.ControllersServices services)
        {
            _context = context;
            _services = services;
        }

        public async Task<List<PasswordResponseDTO>> GetAllAsync(int userId, string masterPassword)
        {
            var passwords = await _context.Passwords
                .Where(p => p.UserID == userId)
                .ToListAsync();

            var result = new List<PasswordResponseDTO>();

            foreach (var p in passwords)
            {
                var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);
                if (key == null)
                    return new List<PasswordResponseDTO>(); 

                result.Add(new PasswordResponseDTO
                {
                    AppName = p.AppName,
                    AppUsername = p.AppUsername,
                    Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                    CreatedAt = p.CreatedAt,
                    LastUpdateAt = p.LastUpdateAt
                });
            }

            return result;
        }

        public async Task<PasswordResponseDTO?> GetByIdAsync(int id, int userId, string masterPassword)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null || p.UserID != userId)
                return null;

            var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);

            if (key == null)
                return null;

            return new PasswordResponseDTO
            {
                AppName = p.AppName,
                AppUsername = p.AppUsername,
                Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                CreatedAt = p.CreatedAt,
                LastUpdateAt = p.LastUpdateAt
            };
        }

        public async Task<PasswordResponseDTO?> GetByAppAsync(string app, int userId, string masterPassword)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return null;

            var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);
            if (key == null)
                return null;

            return new PasswordResponseDTO
            {
                AppName = p.AppName,
                AppUsername = p.AppUsername,
                Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                CreatedAt = p.CreatedAt,
                LastUpdateAt = p.LastUpdateAt
            };
        }

        public async Task<Password> CreateAsync(UpdatePasswordDTO dto, int userId)
        {
            var existing = await _context.Passwords
                .FirstOrDefaultAsync(p => p.AppName == dto.AppName && p.UserID == userId);

            if (existing != null)
                return null;

            var (key, salt) = await _services.KeyFromPassword(dto.MasterPassword, userId);

            var newPassword = new Password
            {
                UserID = userId,
                AppName = dto.AppName,
                AppUsername = dto.AppUsername,
                EncryptedPassword = CryptographyService.Encrypt(dto.Password, key),
                KeySalt = salt,
                CreatedAt = DateTime.UtcNow,
                LastUpdateAt = DateTime.UtcNow
            };

            _context.Passwords.Add(newPassword);
            await _context.SaveChangesAsync();

            return newPassword;
        }

        public async Task<bool> UpdateByIdAsync(int id, UpdatePasswordDTO dto, int userId)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null || p.UserID != userId)
                return false;

            var (key, _) = await _services.KeyFromPassword(dto.MasterPassword, userId, p.KeySalt);
            if (key == null)
                return false;

            if (dto.AppName != null)
                p.AppName = dto.AppName;

            if (dto.AppUsername != null)
                p.AppUsername = dto.AppUsername;

            if (dto.Password != null)
                p.EncryptedPassword = CryptographyService.Encrypt(dto.Password, key);

            p.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateByAppAsync(string app, UpdatePasswordDTO dto, int userId)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == dto.AppName && x.UserID == userId);

            if (p == null)
                return false;

            var (key, _) = await _services.KeyFromPassword(dto.MasterPassword, userId, p.KeySalt);

            if (key == null)
                return false;

            if (dto.AppName != null)
                p.AppName = dto.AppName;

            if (dto.AppUsername != null)
                p.AppUsername = dto.AppUsername;

            if (dto.Password != null)
                p.EncryptedPassword = CryptographyService.Encrypt(dto.Password, key);

            p.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByIdAsync(int id, int userId)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null || p.UserID != userId)
                return false;

            _context.Passwords.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteByAppAsync(string app, int userId)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return false;

            _context.Passwords.Remove(p);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}