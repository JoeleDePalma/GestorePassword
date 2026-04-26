using Libreria.HTTPRequestsLibrary;
using Libreria.HTTPRequestsLibrary.Interfaces;
using Libreria.HTTPRequestsLibrary.Services;
using Libreria.DTOs.Passwords;
using System.Net.Http.Json;

namespace Libreria.API
{
    public class PasswordApi : IPasswordApi
    {
        private readonly ApiClient _client;

        public PasswordApi(ApiClient client)
        {
            _client = client;
        }

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
        public async Task<ApiResponse<List<PasswordResponseDTO>>> GetAllAsync(string masterPassword)
            => await HTTPRequestHelper.SendAsync<List<PasswordResponseDTO>>(
                () => _client.Http.GetAsync($"api/passwords/get/all?masterPassword={Uri.EscapeDataString(masterPassword)}")
                );

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
        public async Task<ApiResponse<PasswordResponseDTO>> GetByIdAsync(int id, string masterPassword)
            => await HTTPRequestHelper.SendAsync<PasswordResponseDTO>(
                () => _client.Http.GetAsync($"api/passwords/get/ById/{id}?masterPassword={Uri.EscapeDataString(masterPassword)}")
                );

        /// <summary>
        /// Sends a POST request to the server to create a new password using the provided data
        /// </summary>
        /// <param name="newPassword">
        /// The DTO that contains the credentials and information of the password to create
        /// </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="CreatedPasswordDTO"/> object
        /// </returns>
        public async Task<ApiResponse<CreatedPasswordDTO>> CreateAsync(CreatePasswordDTO newPassword)
            => await HTTPRequestHelper.SendAsync<CreatedPasswordDTO>(
                () => _client.Http.PostAsJsonAsync("api/passwords/create", newPassword)
                );

        /// <summary>
        /// Sends a PUT request to the server to update an existing password with the specified ID
        /// </summary>
        /// <param name="id">The ID of the password to update </param>
        /// <param name="ModifiedPassword">
        /// The DTO that contains the updated data of the password
        /// </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the update was successful
        /// </returns>
        public async Task<ApiResponse<UpdatedPasswordDTO>> UpdateByIdAsync(int id, UpdatePasswordDTO modifiedPassword)
            => await HTTPRequestHelper.SendAsync<UpdatedPasswordDTO>(
                () => _client.Http.PutAsJsonAsync($"api/passwords/update/ById/{id}", modifiedPassword)
                );

        /// <summary>
        /// Sends a DELETE request to the server to remove the password with the specified ID 
        /// </summary>
        /// <param name="id">The ID of the password to delete </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the delete operation was successful
        /// </returns>
        public async Task<ApiResponse<bool>> DeleteByIdAsync(int id)
            => await HTTPRequestHelper.SendAsync<bool>(
                () => _client.Http.DeleteAsync($"api/passwords/delete/ById/{id}")
                );
    }
}