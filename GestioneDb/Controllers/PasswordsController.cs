using GestioneDb.DTOs.Passwords;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestioneDb.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        private readonly IPasswordService _passwordService;

        public PasswordsController(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        /// <summary>
        /// Returns all passwords saved by the user
        /// </summary>
        /// <param name="masterPassword">
        /// The master password used to decrypt the saved passwords
        /// </param>
        /// <returns>A list of decrypted passwords</returns>
        [HttpGet("get/all")]
        public async Task<Result<List<PasswordResponseDTO>>> GetAllPasswords(string masterPassword)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.GetAllPasswordsAsync(userId, masterPassword);

            return result;
        }

        /// <summary>
        /// Returns the password with the specified ID for the authenticated user.
        /// </summary>
        /// <param name="id">The ID of the requested password.</param>
        /// <param name="masterPassword">The master password used to decrypt the saved password.</param>
        /// <returns>The decrypted password</returns>
        [HttpGet("get/ById/{id}")]
        public async Task<Result<PasswordResponseDTO>> GetPasswordById(int id, string masterPassword)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.GetPasswordByIdAsync(id, userId, masterPassword);

            return result;
        }

        /// <summary>
        /// Creates a new password entry with the provided details and returns the data needed by the client
        /// </summary>
        /// <param name="newPassword">DTO containing the details of the password to be saved</param>
        /// <returns>The details of the saved password for the client</returns>
        [HttpPost("create")]
        public async Task<Result<CreatedPasswordDTO>> CreatePassword(CreatePasswordDTO newPassword)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.CreatePasswordAsync(newPassword, userId);

            return result;
        }

        /// <summary>
        /// Updates a saved password with the specified ID and returns a boolean indicating whether the operation was successful
        /// </summary>
        /// <param name="id">The ID of the password to update</param>
        /// <param name="dto">DTO containing the updated data</param>
        /// <returns>A boolean result.</returns>
        [HttpPut("update/ById/{id}")]
        public async Task<Result<bool>> UpdatePasswordById(int id, UpdatePasswordDTO dto)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.UpdatePasswordByIdAsync(id, dto, userId);

            return result;
        }

        /// <summary>
        /// Deletes a saved password with the specified ID and returns a boolean indicating the operation was successful
        /// </summary>
        /// <param name="id">The ID of the password to delete</param>
        /// <returns>A boolean indicating the operation was successful</returns>
        [HttpDelete("delete/ById/{id}")]
        public async Task<Result<bool>> DeletePasswordById(int id)
        {
            int userId = (int) HttpContext.Items["UserId"];
            var result = await _passwordService.DeletePasswordByIdAsync(id, userId);

            return result;
        }
    }
}