using GestioneDb.Data;
using GestioneDb.DTOs.Passwords;
using GestioneDb.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Security;

namespace GestioneDb.Services.Common.ControllerServices
{
    public class CommonFunctions
    {
        /// <summary>
        /// Retrieves all passwords belonging to the specified user and decrypts them using the provided master password
        /// </summary>
        /// <param name="userId">The ID of the user whose passwords must be retrieved </param>
        /// <param name="masterPassword">The master password used to derive the decryption key </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a list of decrypted <see cref="PasswordResponseDTO"/> objects
        /// if the operation succeeds, or an error result if the master password is invalid
        /// </returns>
        public static async Task<Result<List<PasswordResponseDTO>>> GetAllPasswords(ApplicationDbContext _context, ControllersServices.ControllersServices _services, int userId, string masterPassword)
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

                result.Add
                (
                    new PasswordResponseDTO
                    {
                        Id = p.CredentialID,
                        AppName = p.AppName,
                        AppUsername = p.AppUsername!,
                        Password = CryptographyService.Decrypt(p.EncryptedPassword, key),
                        KeySalt = p.KeySalt,
                        CreatedAt = p.CreatedAt,
                        LastUpdateAt = p.LastUpdateAt
                    }
                );
            }

            return Result<List<PasswordResponseDTO>>.Ok(result, StatusCode.Ok);
        }
    }
}
