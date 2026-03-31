using GestioneDb.Data;
using GestioneDb.DTOs.Passwords;
using GestioneDb.Models;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Security;
using System.Xml;

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
                    return Result<List<PasswordResponseDTO>>.Fail(StatusCode.Unauthorized); 

                result.Add(new PasswordResponseDTO
                {
                    Id = p.CredentialID,
                    AppName = p.AppName,
                    AppUsername = p.AppUsername,
                    Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                    CreatedAt = p.CreatedAt,
                    LastUpdateAt = p.LastUpdateAt
                });
            }

            return Result<List<PasswordResponseDTO>>.Ok(result, StatusCode.Ok);
        }

        public async Task<Result<PasswordResponseDTO?>> GetPasswordByIdAsync(int id, int userId, string masterPassword)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.NotFound);

            if (p.UserID != userId)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.Unauthorized);

            var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.Unauthorized);

            var PasswordInfo = new PasswordResponseDTO()
            {
                Id = p.CredentialID,
                AppName = p.AppName,
                AppUsername = p.AppUsername,
                Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                CreatedAt = p.CreatedAt,
                LastUpdateAt = p.LastUpdateAt
            };

            return Result<PasswordResponseDTO?>.Ok(PasswordInfo, StatusCode.Ok);
        }

        public async Task<Result<PasswordResponseDTO?>> GetPasswordByAppAsync(string app, int userId, string masterPassword)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.NotFound);

            var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.Unauthorized); ;

            var PasswordInfo = new PasswordResponseDTO()
            {
                Id = p.CredentialID,
                AppName = p.AppName,
                AppUsername = p.AppUsername,
                Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                CreatedAt = p.CreatedAt,
                LastUpdateAt = p.LastUpdateAt
            };

            return Result<PasswordResponseDTO?>.Ok(PasswordInfo, StatusCode.Ok);
        }

        public async Task<Result<CreatedPasswordDTO>> CreatePasswordAsync(CreatePasswordDTO dto, int userId)
        {
            var existing = await _context.Passwords
                .FirstOrDefaultAsync(p => p.AppName == dto.AppName && p.UserID == userId);

            if (existing != null)
                return Result<CreatedPasswordDTO>.Fail(StatusCode.BadRequest);

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

            var NewPasswordInfo = new CreatedPasswordDTO()
            {
                CredentialID = newPassword.CredentialID,
                AppName = newPassword.AppName,
                AppUsername = newPassword.AppUsername,
                Password = dto.Password,
                CreatedAt = newPassword.CreatedAt,
                LastUpdateAt = newPassword.LastUpdateAt
            };

            return Result<CreatedPasswordDTO?>.Ok(NewPasswordInfo, StatusCode.Created);
        }

        public async Task<Result<bool>> UpdatePasswordByIdAsync(int id, UpdatePasswordDTO dto, int userId)
        {
            var existing = await _context.Passwords
                .FirstOrDefaultAsync(p => p.AppName == dto.AppName && p.UserID == userId && p.CredentialID != id);

            if (existing != null)
                return Result<bool>.Fail(StatusCode.BadRequest);

            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<bool>.Fail(StatusCode.NotFound);

            if (p.UserID != userId)
                return Result<bool>.Fail(StatusCode.Unauthorized);

            var (key, _) = await _services.KeyFromPassword(dto.MasterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<bool>.Fail(StatusCode.Unauthorized);

            if (dto.AppName != null)
                p.AppName = dto.AppName;

            if (dto.AppUsername != null)
                p.AppUsername = dto.AppUsername;

            if (dto.Password != null)
                p.EncryptedPassword = CryptographyService.Encrypt(dto.Password, key);

            p.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true, StatusCode.NoContent);
        }

        public async Task<Result<bool>> UpdatePasswordByAppAsync(string app, UpdatePasswordDTO dto, int userId)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return Result<bool>.Fail(StatusCode.NotFound);

            var (key, _) = await _services.KeyFromPassword(dto.MasterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<bool>.Fail(StatusCode.Unauthorized);

            if (dto.AppName != null)
                p.AppName = dto.AppName;

            if (dto.AppUsername != null)
                p.AppUsername = dto.AppUsername;

            if (dto.Password != null)
                p.EncryptedPassword = CryptographyService.Encrypt(dto.Password, key);

            p.LastUpdateAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true, StatusCode.NoContent);
        }

        public async Task<Result<bool>> DeletePasswordByIdAsync(int id, int userId)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<bool>.Fail(StatusCode.NotFound);

            if (p.UserID != userId)
                return Result<bool>.Fail(StatusCode.Unauthorized);

            _context.Passwords.Remove(p);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true, StatusCode.NoContent);
        }

        public async Task<Result<bool>> DeletePasswordByAppAsync(string app, int userId)
        {
            var p = await _context.Passwords
                .FirstOrDefaultAsync(x => x.AppName == app && x.UserID == userId);

            if (p == null)
                return Result<bool>.Fail(StatusCode.NotFound);

            _context.Passwords.Remove(p);
            await _context.SaveChangesAsync();
            return Result<bool>.Ok(true, StatusCode.NoContent);
        }
    }
}