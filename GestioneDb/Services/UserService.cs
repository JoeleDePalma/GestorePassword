using GestioneDb.Data;
using GestioneDb.DTOs.Users;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Security;

namespace GestioneDb.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ControllersServices.ControllersServices _services;

        public UserService(ApplicationDbContext context, ControllersServices.ControllersServices services)
        {
            _context = context;
            _services = services;
        }

        /// <summary>
        /// Retrieves the data of the user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="UserResponseDTO"/> object
        /// if the operation succeeds, or an error result if the user does not exist.
        /// </returns>
        public async Task<Result<UserResponseDTO>> GetUserByIdAsync(int id)
        {
            var Response = await _context.Users.FindAsync(id);

            if (Response == null)
                return Result<UserResponseDTO>.Fail(StatusCode.NotFound, "User not found");

            var user = new UserResponseDTO()
            {
                UserID = (int) Response.UserID,
                Username = Response.Username,
                CreatedAt = Response.CreatedAt
            };

            return Result<UserResponseDTO>.Ok(user, StatusCode.Ok);
        }

        /// <summary>
        /// Registers a new user and generates a JWT token for authentication
        /// </summary>
        /// <param name="credentials">The registration data provided by the user </param>
        /// <param name="jwt">The JWT service used to generate the authentication token </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the created <see cref="RegisterResponseDTO"/> object
        /// if the registration succeeds, or an error result if the username is already taken
        /// or if the password hashing process fails
        /// </returns>
        public async Task<Result<RegisterResponseDTO>> CreateUserAsync(RegisterDTO credentials, JwtService jwt)
        {
            if (credentials == null)
                return Result<RegisterResponseDTO>.Fail(StatusCode.BadRequest, "Credentials aren't valid");

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == credentials.Username.ToLower());

            if (existing != null)
                return Result<RegisterResponseDTO>.Fail(StatusCode.BadRequest, "This username already exists");

            string HashedPassword, Salt;

            try
            {
                (HashedPassword, Salt) = HashingService.HashPassword(credentials.Password);
            }
            catch (Exception)
            {
                return Result<RegisterResponseDTO>.Fail(StatusCode.InternalServerError, "Internal server error during the password hashing");
            }

            var NewUser = new Models.User
            {
                Username = credentials.Username,
                HashedPassword = HashedPassword,
                PasswordSalt = Salt,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            var tokenUser = new Models.User
            {
                UserID = NewUser.UserID,
                Username = credentials.Username,
                HashedPassword = HashedPassword,
                PasswordSalt = Salt,
                CreatedAt = NewUser.CreatedAt
            };

            var token = jwt.GenerateToken(tokenUser);

            var response = new RegisterResponseDTO
            {
                UserID = (int) NewUser.UserID,
                Username = NewUser.Username,
                CreatedAt = NewUser.CreatedAt,
                Token = token
            };

            return Result<RegisterResponseDTO>.Ok(response, StatusCode.Created);
        }

        /// <summary>
        /// Updates the specified user's data, hashing the new password if provided
        /// </summary>
        /// <param name="id">The ID of the user to update </param>
        /// <param name="ModifiedUser">The data to update for the user </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a boolean value indicating whether the update was successful,
        /// or an error result if the user does not exist, the username is already taken,
        /// or the password hashing process fails
        /// </returns>
        public async Task<Result<bool>> UpdateUserByIdAsync(int id, UpdateUserDTO ModifiedUser)
        {
            if (ModifiedUser == null)
                return Result<bool>.Fail(StatusCode.BadRequest, "The informations are null");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return Result<bool>.Fail(StatusCode.NotFound, "User not found");

            if (!string.IsNullOrEmpty(ModifiedUser.Username))
            {
                var conflict = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username.ToLower() == ModifiedUser.Username.ToLower() && u.UserID != id);

                if (conflict != null)
                {
                    return Result<bool>.Fail(StatusCode.BadRequest, "This username already exists");
                }

                user.Username = ModifiedUser.Username;
            }

            if (!string.IsNullOrEmpty(ModifiedUser.Password))
            {
                try
                {
                    (user.HashedPassword, user.PasswordSalt) = HashingService.HashPassword(ModifiedUser.Password);
                }
                catch (Exception)
                {
                    return Result<bool>.Fail(StatusCode.InternalServerError, "Internal server error during the password hashing");
                }
            }

            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true, StatusCode.Ok);
        }

        /// <summary>
        /// Deletes the specified user
        /// </summary>
        /// <param name="id">The ID of the user to delete </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a boolean value indicating whether the deletion was successful,
        /// or an error result if the user does not exist
        /// </returns>
        public async Task<Result<bool>> DeleteUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return Result<bool>.Fail(StatusCode.NotFound, "User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true, StatusCode.NoContent);
        }

        /// <summary>
        /// Validates the provided credentials and generates a JWT token for the authenticated user
        /// </summary>
        /// <param name="credentials">The login data provided by the user </param>
        /// <param name="jwt">The JWT service used to generate the authentication token </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing the <see cref="LoginResponseDTO"/> object
        /// if the authentication succeeds, or an error result if the user does not exist
        /// or the provided password is incorrect
        /// </returns>
        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO credentials, JwtService jwt)
        {
            if (credentials == null)
                return Result<LoginResponseDTO>.Fail(StatusCode.BadRequest, "Credential are null");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == credentials.Username.ToLower());

            if (user == null)
                return Result<LoginResponseDTO>.Fail(StatusCode.NotFound, "User not found");

            bool ok = HashingService.VerifyPassword(
                credentials.Password,
                user.HashedPassword,
                user.PasswordSalt
            );

            if (!ok)
                return Result<LoginResponseDTO>.Fail(StatusCode.Unauthorized, "Wrong password");

            var tokenUser = new Models.User
            {
                UserID = user.UserID,
                Username = user.Username,
                HashedPassword = user.HashedPassword,
                PasswordSalt = user.PasswordSalt,
                CreatedAt = user.CreatedAt
            };

            var token = jwt.GenerateToken(tokenUser);

            var obj = new LoginResponseDTO()
            {
                UserID = (int) user.UserID,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                Token = token
            };

            return Result<LoginResponseDTO>.Ok(obj, StatusCode.Ok);
        }
    }
}
