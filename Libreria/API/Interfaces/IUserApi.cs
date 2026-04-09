using Libreria.HTTPRequestsLibrary.Services;
using Libreria.DTOs.Users;

namespace Libreria.HTTPRequestsLibrary.Interfaces
{
    public interface IUserApi
    {
        /// <summary>
        /// Sends a GET request to the server to retrieve the user with the specified ID
        /// </summary>
        /// <param name="id">The ID of the user to retrieve </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="UserResponseDTO"/> object
        /// </returns>
        Task<ApiResponse<UserResponseDTO>> GetByIdAsync(int id);

        /// <summary>
        /// Sends a GET request to the server to retrieve the user linked to the current JWT token
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="UserResponseDTO"/> object
        /// </returns>
        Task<ApiResponse<UserResponseDTO>> GetByTokenAsync();


        /// <summary>
        /// Sends a POST request to the server to register a new user with the provided credentials
        /// </summary>
        /// <param name="Credentials">The DTO that contains the registration data </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="RegisterResponseDTO"/> object
        /// </returns>
        Task<ApiResponse<RegisterResponseDTO>> RegisterAsync(RegisterDTO Credentials);

        /// <summary>
        /// Sends a PUT request to the server to update the data of the current user
        /// </summary>
        /// <param name="ModifiedUser">The DTO that contains the updated user data </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the update was successful
        /// </returns>
        Task<ApiResponse<bool>> UpdateByIdAsync(UpdateUserDTO ModifiedUser);


        /// <summary>
        /// Sends a DELETE request to the server to remove the user linked to the current JWT token
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the delete operation was successful
        /// </returns>
        Task<ApiResponse<bool>> DeleteByTokenAsync();


        /// <summary>
        /// Sends a POST request to the server to authenticate the user with the provided credentials
        /// </summary>
        /// <param name="dto">The DTO that contains the login credentials </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="LoginResponseDTO"/> object
        /// </returns>
        Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO dto);
    }
}
