using GestioneDb.DTOs.Users;
using GestioneDb.Models;
using Microsoft.AspNetCore.Mvc;
using GestioneDb.Services.Common;

namespace GestioneDb.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponseDTO>> GetUserByIdAsync(int id);

        Task<Result<RegisterResponseDTO>> CreateUserAsync(RegisterDTO Credentials, JwtService jwt);
        Task<Result<bool>> UpdateUserByIdAsync(int id, [FromBody] UpdateUserDTO ModifiedUser);

        Task<Result<bool>> DeleteUserByIdAsync(int id);

        Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO credentials, JwtService jwt);

    }
}
