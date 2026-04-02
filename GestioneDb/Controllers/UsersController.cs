using GestioneDb.Data;
using GestioneDb.DTOs.Users;
using GestioneDb.Middlewares;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestioneDb.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Returns the data of a user with the specified ID
        /// </summary>
        /// <param name="id">The ID of the user to retrieve</param>
        /// <returns>User data</returns>
        [SkipUserIdExtraction]
        [AllowAnonymous]
        [HttpGet("get/ById/{id}")]
        public async Task<Result<UserResponseDTO>> GetUserById(int id)
        {
            var result = await _userService.GetUserByIdAsync(id);

            return result;
        }

        /// <summary>
        /// Returns the data of the user whose ID was extracted from the authentication token
        /// </summary>
        /// <returns>User data</returns>
        [HttpGet("get/ByToken")]
        public async Task<Result<UserResponseDTO>> GetUserByToken()
        {
            int id = (int) HttpContext.Items["UserId"];
            var result = await _userService.GetUserByIdAsync(id);

            return result;
        }

        /// <summary>
        /// Creates a new user and returns the data for the client
        /// </summary>
        /// <param name="newUser">DTO with the new user information to save</param>
        /// <param name="jwt">JWT configuration service</param>
        /// <returns>User data</returns>
        [SkipUserIdExtraction]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<Result<RegisterResponseDTO>> CreateUser([FromBody] RegisterDTO newUser, [FromServices] JwtService jwt)
        {
            var result = await _userService.CreateUserAsync(newUser, jwt);

            return result;
        }

        /// <summary>
        /// Updates the user data and returns a boolean result indicating the operation was successful
        /// </summary>
        /// <param name="modifiedUser">DTO with updated data</param>
        /// <returns>A boolean result indicating the operation was successful</returns>
        [HttpPut("update")]
        public async Task<Result<bool>> UpdateUser([FromBody] UpdateUserDTO modifiedUser)
        {
            int id = (int) HttpContext.Items["UserId"];
            var result = await _userService.UpdateUserByIdAsync(id, modifiedUser);

            return result;
        }

        /// <summary>
        /// Deletes the user whose ID was extracted from the JWT token
        /// </summary>
        /// <returns>A boolean result indicating the operation was successful</returns>
        [HttpDelete("delete")]
        public async Task<Result<bool>> DeleteUserByToken()
        {
            int id = (int) HttpContext.Items["UserId"];
            var result = await _userService.DeleteUserByIdAsync(id);

            return result;
        }

        /// <summary>
        /// Validates the credentials and returns the user data with a JWT token
        /// </summary>
        /// <param name="credentials">Credentials used for login</param>
        /// <param name="jwt">JWT configuration service</param>
        /// <returns>User data with a JWT token</returns>
        [SkipUserIdExtraction]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<Result<LoginResponseDTO>> Login(LoginDTO credentials, [FromServices] JwtService jwt)
        {
            var result = await _userService.LoginAsync(credentials, jwt);

            return result;
        }
    } 
}
