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

        /// <summary>
        /// Retrieves all passwords belonging to the specified user and decrypts them using the provided master password
        /// </summary>
        /// <param name="userId">The ID of the user whose passwords must be retrieved </param>
        /// <param name="masterPassword">The master password used to derive the decryption key </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a list of decrypted <see cref="PasswordResponseDTO"/> objects
        /// if the operation succeeds, or an error result if the master password is invalid
        /// </returns>
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
                    return Result<List<PasswordResponseDTO>>.Fail(StatusCode.Unauthorized, "Chiave crittografica invalida"); 

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

        /// <summary>
        /// Retrieves a specific password belonging to the specified user and decrypts it using the provided master password
        /// </summary>
        /// <param name="id">The ID of the password to retrieve </param>
        /// <param name="userId">The ID of the user who owns the password </param>
        /// <param name="masterPassword">The master password used to derive the decryption key </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the decrypted <see cref="PasswordResponseDTO"/> object
        /// if the operation succeeds, or an error result if the password does not exist,
        /// does not belong to the user, or the master password is invalid
        /// </returns>
        public async Task<Result<PasswordResponseDTO?>> GetPasswordByIdAsync(int id, int userId, string masterPassword)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.NotFound, "Password non trovata");

            if (p.UserID != userId)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.Unauthorized, "Password appartenente ad un altro account");

            var (key, _) = await _services.KeyFromPassword(masterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<PasswordResponseDTO?>.Fail(StatusCode.Unauthorized, "Chiave crittografica invalida");

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

        /// <summary>
        /// Creates a new password for the specified user, encrypting it using the master password provided in the request
        /// </summary>
        /// <param name="dto">The data required to create the password </param>
        /// <param name="userId">The ID of the user creating the password </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the created <see cref="CreatedPasswordDTO"/> object
        /// if the operation succeeds, or an error result if the encryption process fails
        /// </returns>
        public async Task<Result<CreatedPasswordDTO>> CreatePasswordAsync(CreatePasswordDTO dto, int userId)
        {
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

            return Result<CreatedPasswordDTO>.Ok(NewPasswordInfo, StatusCode.Created);
        }

        /// <summary>
        /// Updates the specified password for the given user, encrypting the new password if provided
        /// </summary>
        /// <param name="id">The ID of the password to update </param>
        /// <param name="dto">The data to update for the password </param>
        /// <param name="userId">The ID of the user performing the update </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a boolean value indicating whether the update was successful,
        /// or an error result if the password does not exist, does not belong to the user,
        /// or the master password is invalid
        /// </returns>
        public async Task<Result<bool>> UpdatePasswordByIdAsync(int id, UpdatePasswordDTO dto, int userId)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<bool>.Fail(StatusCode.NotFound, "Password non trovata");

            if (p.UserID != userId)
                return Result<bool>.Fail(StatusCode.Unauthorized, "Password appartenente ad un altro account");

            var (key, _) = await _services.KeyFromPassword(dto.MasterPassword, userId, p.KeySalt);

            if (key == null)
                return Result<bool>.Fail(StatusCode.Unauthorized, "Chiave crittografica invalida");

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

        /// <summary>
        /// Deletes the specified password for the given user
        /// </summary>
        /// <param name="id">The ID of the password to delete </param>
        /// <param name="userId">The ID of the user performing the deletion </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a boolean value indicating whether the deletion was successful,
        /// or an error result if the password does not exist or does not belong to the user
        /// </returns>
        public async Task<Result<bool>> DeletePasswordByIdAsync(int id, int userId)
        {
            var p = await _context.Passwords.FindAsync(id);

            if (p == null)
                return Result<bool>.Fail(StatusCode.NotFound, "Password non trovata");

            if (p.UserID != userId)
                return Result<bool>.Fail(StatusCode.Unauthorized, "Password appartenente ad un altro utente");

            _context.Passwords.Remove(p);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true, StatusCode.NoContent);
        }
    }
}