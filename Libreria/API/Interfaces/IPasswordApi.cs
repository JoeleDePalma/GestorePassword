using Libreria.HTTPRequestsLibrary.Services;
using Libreria.DTOs.Passwords;

namespace Libreria.HTTPRequestsLibrary.Interfaces
{
    public interface IPasswordApi
    {
        /// <summary>
        /// Sends a GET request to the server to retrieve all saved passwords
        /// </summary>
        /// <param name="masterPassword">
        /// The master password used by the server to decrypt the stored passwords
        /// </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="List{T}"/> of
        /// <see cref="PasswordResponseDTO"/> objects
        /// </returns>
        Task<ApiResponse<List<PasswordResponseDTO>>> GetAllAsync(string masterPassword);

        /// <summary>
        /// Sends a GET request to the server to retrieve the password with the specified ID
        /// </summary>
        /// <param name="id">The ID of the password to retrieve </param>
        /// <param name="masterPassword">
        /// The master password used by the server to decrypt the stored password
        /// </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="PasswordResponseDTO"/> object
        /// </returns>
        Task<ApiResponse<PasswordResponseDTO>> GetByIdAsync(int id, string masterPassword);


        /// <summary>
        /// Sends a POST request to the server to create a new password using the provided data
        /// </summary>
        /// <param name="newPassword">
        /// The DTO that contains the credentials and information of the password to create
        /// </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="CreatedPasswordDTO"/> object
        /// </returns>
        Task<ApiResponse<CreatedPasswordDTO>> CreateAsync(CreatePasswordDTO newPassword);

        /// <summary>
        /// Sends a PUT request to the server to update an existing password with the specified ID
        /// </summary>
        /// <param name="id">The ID of the password to update </param>
        /// <param name="modifiedPassword">
        /// The DTO that contains the updated data of the password
        /// </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the update was successful
        /// </returns>
        Task<ApiResponse<UpdatedPasswordDTO>> UpdateByIdAsync(int id, UpdatePasswordDTO modifiedPassword);


        /// <summary>
        /// Sends a DELETE request to the server to remove the password with the specified ID
        /// </summary>
        /// <param name="id">The ID of the password to delete </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the delete operation was successful 
        /// </returns>
        Task<ApiResponse<bool>> DeleteByIdAsync(int id);
    }
}