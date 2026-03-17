using HTTPRequestsLibrary;
using HTTPRequestsLibrary.Interfaces;
using HTTPRequestsLibrary.Services;
using Libreria.DTOs.Passwords;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace Libreria.API
{
    public class PasswordApi : IPasswordApi
    {
        private readonly ApiClient _client;

        public PasswordApi(ApiClient client)
        {
            _client = client;
        }

        public async Task<ApiResponse<List<PasswordResponseDTO>>> GetAllAsync(string masterPassword)
            => await HTTPRequestHelper.SendAsync<List<PasswordResponseDTO>>(() => _client.Http.GetAsync($"api/passwords/get/all?masterPassword={Uri.EscapeDataString(masterPassword)}"));

        public async Task<ApiResponse<PasswordResponseDTO>> GetByIdAsync(int id, string masterPassword)
            => await HTTPRequestHelper.SendAsync<PasswordResponseDTO>(() => _client.Http.GetAsync($"api/passwords/get/ById/{id}?masterPassword={Uri.EscapeDataString(masterPassword)}"));

        public async Task<ApiResponse<PasswordResponseDTO>> GetByAppAsync(string app, string masterPassword)
            => await HTTPRequestHelper.SendAsync<PasswordResponseDTO>(() => _client.Http.GetAsync($"api/passwords/get/ByApp/{Uri.EscapeDataString(app)}?masterPassword={Uri.EscapeDataString(masterPassword)}"));

        public async Task<ApiResponse<CreatedPasswordDTO>> CreateAsync(CreatePasswordDTO NewPassword)
            => await HTTPRequestHelper.SendAsync<CreatedPasswordDTO>(() => _client.Http.PostAsJsonAsync("api/passwords/create", NewPassword));

        public async Task<ApiResponse<bool>> UpdateByIdAsync(int id,UpdatePasswordDTO ModifiedPassword)
            => await HTTPRequestHelper.SendAsync<bool>(() => _client.Http.PutAsJsonAsync($"api/passwords/update/ById/{id}", ModifiedPassword));
       
        public async Task<ApiResponse<bool>> UpdateByAppAsync(string app, UpdatePasswordDTO ModifiedPassword)
            => await HTTPRequestHelper.SendAsync<bool>(() => _client.Http.PutAsJsonAsync($"api/passwords/update/ByApp/{app}", ModifiedPassword));

        public async Task<ApiResponse<bool>> DeleteByIdAsync(int id)
            => await HTTPRequestHelper.SendAsync<bool>(() => _client.Http.DeleteAsync($"api/passwords/delete/ById/{id}"));
        
        public async Task<ApiResponse<bool>> DeleteByAppAsync(string app)
            => await HTTPRequestHelper.SendAsync<bool>(() => _client.Http.DeleteAsync($"api/passwords/delete/ByApp/{app}"));
    }
}
