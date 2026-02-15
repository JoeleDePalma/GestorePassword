using GestioneDb.Data;
using GestioneDb.DTOs.Passwords;
using GestioneDb.Models;
using GestioneDb.Services.Common;
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

        public async Task<Result<List<PasswordResponseDTO>>> GetAllPasswordsAsync(int userId, string masterPassword)
        {
            var passwords = await _context.Passwords
                .Where(p => p.UserID == userId)
                .ToListAsync();

            var result = new List<PasswordResponseDTO>();

            foreach (var p in passwords)
            {
                var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);

                if (key == null)
                    return Result<List<PasswordResponseDTO>>.Fail(ErrorCode.Unauthorized); 

                result.Add(new PasswordResponseDTO
                {
                    AppName = p.AppName,
                    AppUsername = p.AppUsername,
                    Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                    CreatedAt = p.CreatedAt,
                    LastUpdateAt = p.LastUpdateAt
                });
            }

            return Result<List<PasswordResponseDTO>>.Ok(result);
        }

        public async Task<Result<PasswordResponseDTO?>> GetPasswordByIdAsync(int id, int userId, string masterPassword)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<PasswordResponseDTO?>.Fail(ErrorCode.NotFound);

            if (p.UserID != userId)
                return Result<PasswordResponseDTO?>.Fail(ErrorCode.Unauthorized);

            var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<PasswordResponseDTO?>.Fail(ErrorCode.Unauthorized);

            var PasswordInfo = new PasswordResponseDTO()
            {
                AppName = p.AppName,
                AppUsername = p.AppUsername,
                Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                CreatedAt = p.CreatedAt,
                LastUpdateAt = p.LastUpdateAt
            };

            return Result<PasswordResponseDTO?>.Ok(PasswordInfo);
        }

        public async Task<Result<PasswordResponseDTO?>> GetPasswordByAppAsync(string app, int userId, string masterPassword)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return Result<PasswordResponseDTO?>.Fail(ErrorCode.NotFound);

            var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<PasswordResponseDTO?>.Fail(ErrorCode.Unauthorized); ;

            var PasswordInfo = new PasswordResponseDTO()
            {
                AppName = p.AppName,
                AppUsername = p.AppUsername,
                Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                CreatedAt = p.CreatedAt,
                LastUpdateAt = p.LastUpdateAt
            };

            return Result<PasswordResponseDTO?>.Ok(PasswordInfo);
        }

        public async Task<Result<Password>> CreatePasswordAsync(UpdatePasswordDTO dto, int userId)
        {
            var existing = await _context.Passwords
                .FirstOrDefaultAsync(p => p.AppName == dto.AppName && p.UserID == userId);

            if (existing != null)
                return Result<Password>.Fail(ErrorCode.BadRequest);

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

            return Result<Password>.Ok(newPassword);
        }

        public async Task<Result<bool>> UpdatePasswordByIdAsync(int id, UpdatePasswordDTO dto, int userId)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<bool>.Fail(ErrorCode.NotFound);

            if (p.UserID != userId)
                return Result<bool>.Fail(ErrorCode.Unauthorized);

            var (key, _) = await _services.KeyFromPassword(dto.MasterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<bool>.Fail(ErrorCode.Unauthorized);

            if (dto.AppName != null)
                p.AppName = dto.AppName;

            if (dto.AppUsername != null)
                p.AppUsername = dto.AppUsername;

            if (dto.Password != null)
                p.EncryptedPassword = CryptographyService.Encrypt(dto.Password, key);

            p.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> UpdatePasswordByAppAsync(string app, UpdatePasswordDTO dto, int userId)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return Result<bool>.Fail(ErrorCode.NotFound);

            var (key, _) = await _services.KeyFromPassword(dto.MasterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<bool>.Fail(ErrorCode.Unauthorized);

            if (dto.AppName != null)
                p.AppName = dto.AppName;

            if (dto.AppUsername != null)
                p.AppUsername = dto.AppUsername;

            if (dto.Password != null)
                p.EncryptedPassword = CryptographyService.Encrypt(dto.Password, key);

            p.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> DeletePasswordByIdAsync(int id, int userId)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<bool>.Fail(ErrorCode.NotFound);

            if (p.UserID != userId)
                return Result<bool>.Fail(ErrorCode.Unauthorized);

            _context.Passwords.Remove(p);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> DeletePasswordByAppAsync(string app, int userId)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return Result<bool>.Fail(ErrorCode.NotFound);

            _context.Passwords.Remove(p);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true);
        }
    }
}