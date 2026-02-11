using GestioneDb.DTOs.Passwords;
using GestioneDb.Models;

namespace GestioneDb.Services.Interfaces
{
    public interface IPasswordService
    {
        Task<List<PasswordResponseDTO>> GetAllAsync(int userId, string masterPassword);
        Task<PasswordResponseDTO?> GetByIdAsync(int id, int userId, string masterPassword);
        Task<PasswordResponseDTO?> GetByAppAsync(string app, int userId, string masterPassword);

        Task<Password> CreateAsync(UpdatePasswordDTO dto, int userId);
        Task<bool> UpdateByIdAsync(int id, UpdatePasswordDTO dto, int userId);
        Task<bool> UpdateByAppAsync(string app, UpdatePasswordDTO dto, int userId);

        Task<bool> DeleteByIdAsync(int id, int userId);
        Task<bool> DeleteByAppAsync(string app, int userId);
    }
}