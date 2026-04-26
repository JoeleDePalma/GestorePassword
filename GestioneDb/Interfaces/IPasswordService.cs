using GestioneDb.DTOs.Passwords;
using GestioneDb.Services.Common;

namespace GestioneDb.Services.Interfaces
{
    public interface IPasswordService
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
        Task<Result<List<PasswordResponseDTO>>> GetAllPasswordsAsync(int userId, string masterPassword);

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
        Task<Result<PasswordResponseDTO?>> GetPasswordByIdAsync(int id, int userId, string masterPassword);


        /// <summary>
        /// Creates a new password for the specified user, encrypting it using the master password provided in the request
        /// </summary>
        /// <param name="dto">The data required to create the password </param>
        /// <param name="userId">The ID of the user creating the password </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the created <see cref="CreatedPasswordDTO"/> object
        /// if the operation succeeds, or an error result if the encryption process fails
        /// </returns>
        Task<Result<CreatedPasswordDTO>> CreatePasswordAsync(CreatePasswordDTO dto, int userId);

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
        Task<Result<UpdatedPasswordDTO>> UpdatePasswordByIdAsync(int id, UpdatePasswordDTO dto, int userId);


        /// <summary>
        /// Deletes the specified password for the given user
        /// </summary>
        /// <param name="id">The ID of the password to delete </param>
        /// <param name="userId">The ID of the user performing the deletion </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a boolean value indicating whether the deletion was successful,
        /// or an error result if the password does not exist or does not belong to the user
        /// </returns>
        Task<Result<bool>> DeletePasswordByIdAsync(int id, int userId);
    }
}