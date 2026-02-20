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

        public async Task<ApiResponse<UserResponseDTO>> GetByIdAsync(int id)
            => await HTTPRequestHelper.SendAsync<UserResponseDTO>(() => _client.Http.GetAsync($"api/users/get/ById/{id}"));

        public async Task<ApiResponse<RegisterResponseDTO>> RegisterAsync(RegisterDTO Credentials)
            => await HTTPRequestHelper.SendAsync<RegisterResponseDTO>(() => _client.Http.PostAsJsonAsync("api/users/register", Credentials));

        public async Task<ApiResponse<bool>> UpdateByIdAsync(UpdateUserDTO ModifiedUser)
            => await HTTPRequestHelper.SendAsync<bool>(() => _client.Http.PutAsJsonAsync("api/users/update", ModifiedUser));

        public async Task<ApiResponse<bool>> DeleteByIdAsync()
            => await HTTPRequestHelper.SendAsync<bool>(() => _client.Http.DeleteAsync("api/users/delete"));

        public async Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO dto)
            => await HTTPRequestHelper.SendAsync<LoginResponseDTO>(() => _client.Http.PostAsJsonAsync("api/users/login", dto));
    }
}