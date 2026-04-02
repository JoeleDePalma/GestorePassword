using HTTPRequestsLibrary;
using HTTPRequestsLibrary.Interfaces;
using HTTPRequestsLibrary.Services;
using Libreria.DTOs.Users;
using System.Net.Http.Json;

namespace Libreria.API
{
    public class UserApi : IUserApi
    {
        private readonly ApiClient _client;

        public UserApi(ApiClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Sends a GET request to the server to retrieve the user with the specified ID
        /// </summary>
        /// <param name="id">The ID of the user to retrieve </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="UserResponseDTO"/> object
        /// </returns>
        public async Task<ApiResponse<UserResponseDTO>> GetByIdAsync(int id)
            => await HTTPRequestHelper.SendAsync<UserResponseDTO>(
                () => _client.Http.GetAsync($"api/users/get/ById/{id}")
                );

        /// <summary>
        /// Sends a GET request to the server to retrieve the user linked to the current JWT token
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="UserResponseDTO"/> object
        /// </returns>
        public async Task<ApiResponse<UserResponseDTO>> GetByTokenAsync()
            => await HTTPRequestHelper.SendAsync<UserResponseDTO>(
                () => _client.Http.GetAsync("api/users/get/ByToken")
                );


        /// <summary>
        /// Sends a POST request to the server to register a new user with the provided credentials
        /// </summary>
        /// <param name="Credentials">The DTO that contains the registration data </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="RegisterResponseDTO"/> object
        /// </returns>
        public async Task<ApiResponse<RegisterResponseDTO>> RegisterAsync(RegisterDTO Credentials)
            => await HTTPRequestHelper.SendAsync<RegisterResponseDTO>(
                () => _client.Http.PostAsJsonAsync("api/users/register", Credentials)
                );


        /// <summary>
        /// Sends a PUT request to the server to update the data of the current user
        /// </summary>
        /// <param name="ModifiedUser">The DTO that contains the updated user data </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the update was successful
        /// </returns>
        public async Task<ApiResponse<bool>> UpdateByIdAsync(UpdateUserDTO ModifiedUser)
            => await HTTPRequestHelper.SendAsync<bool>(
                () => _client.Http.PutAsJsonAsync("api/users/update", ModifiedUser)
                );


        /// <summary>
        /// Sends a DELETE request to the server to remove the user linked to the current JWT token
        /// </summary>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a boolean value that indicates whether the delete operation was successful
        /// </returns>
        public async Task<ApiResponse<bool>> DeleteByTokenAsync()
            => await HTTPRequestHelper.SendAsync<bool>(
                () => _client.Http.DeleteAsync("api/users/delete")
                );


        /// <summary>
        /// Sends a POST request to the server to authenticate the user with the provided credentials
        /// </summary>
        /// <param name="dto">The DTO that contains the login credentials </param>
        /// <returns>
        /// An <see cref="ApiResponse{T}"/> containing a <see cref="LoginResponseDTO"/> object
        /// </returns>
        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO dto)
            => await HTTPRequestHelper.SendAsync<LoginResponseDTO>(
                () => _client.Http.PostAsJsonAsync("api/users/login", dto)
                );
    }
}