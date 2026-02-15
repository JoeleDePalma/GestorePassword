using GestioneDb.DTOs.Passwords;
using GestioneDb.Models;
using GestioneDb.Services.Common;

namespace GestioneDb.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<Result<List<PasswordResponseDTO>>> GetAllPasswordsAsync(int userId, string masterPassword);
        Task<Result<PasswordResponseDTO?>> GetPasswordByIdAsync(int id, int userId, string masterPassword);
        Task<Result<PasswordResponseDTO?>> GetPasswordByAppAsync(string app, int userId, string masterPassword);

        Task<Result<Password>> CreatePasswordAsync(UpdatePasswordDTO dto, int userId);
        Task<Result<bool>> UpdatePasswordByIdAsync(int id, UpdatePasswordDTO dto, int userId);
        Task<Result<bool>> UpdatePasswordByAppAsync(string app, UpdatePasswordDTO dto, int userId);

        Task<Result<bool>> DeletePasswordByIdAsync(int id, int userId);
        Task<Result<bool>> DeletePasswordByAppAsync(string app, int userId);
    }
}