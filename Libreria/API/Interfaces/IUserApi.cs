using HTTPRequestsLibrary.Services;
using Libreria.DTOs.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace HTTPRequestsLibrary.Interfaces
{
    public interface IUserApi
    {
        Task<ApiResponse<UserResponseDTO>> GetByIdAsync(int id);

        Task<ApiResponse<RegisterResponseDTO>> RegisterAsync(RegisterDTO Credentials);

        Task<ApiResponse<bool>> UpdateByIdAsync(UpdateUserDTO ModifiedUser);

        Task<ApiResponse<bool>> DeleteByIdAsync();

        Task<ApiResponse<LoginResponseDTO>> LoginAsync(LoginDTO dto);
    }
}
