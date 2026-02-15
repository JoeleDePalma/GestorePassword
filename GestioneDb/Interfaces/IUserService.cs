using GestioneDb.DTOs.Users;
using GestioneDb.Models;
using Microsoft.AspNetCore.Mvc;
using GestioneDb.Services.Common;

namespace GestioneDb.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result<UserResponseDTO>> GetUserByIdAsync(int id);

        Task<Result<UserResponseDTO>> CreateUserAsync(RegisterDTO Credentials);
        Task<Result<bool>> UpdateUserByIdAsync(int id, [FromBody] UpdateUserDTO ModifiedUser);

        Task<Result<bool>> DeleteUserByIdAsync(int id);

        Task<Result<string>> LoginAsync(LoginDTO credentials, JwtService jwt);

    }
}
