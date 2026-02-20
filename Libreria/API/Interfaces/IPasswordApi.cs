using HTTPRequestsLibrary.Services;
using Libreria.DTOs.Passwords;

namespace HTTPRequestsLibrary.Interfaces
{
    public interface IPasswordApi
    {
        Task<ApiResponse<List<PasswordResponseDTO>>> GetAllAsync(string masterPassword);
        Task<ApiResponse<PasswordResponseDTO>> GetByIdAsync(int id, string masterPassword);
        Task<ApiResponse<PasswordResponseDTO>> GetByAppAsync(string app, string masterPassword);

        Task<ApiResponse<CreatedPasswordDTO>> CreateAsync(UpdatePasswordDTO dto);

        Task<ApiResponse<bool>> UpdateByIdAsync(int id, UpdatePasswordDTO dto);
        Task<ApiResponse<bool>> UpdateByAppAsync(string app, UpdatePasswordDTO dto);

        Task<ApiResponse<bool>> DeleteByIdAsync(int id);
        Task<ApiResponse<bool>> DeleteByAppAsync(string app);
    }
}
