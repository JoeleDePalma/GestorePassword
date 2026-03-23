using GestioneDb.Data;
using GestioneDb.DTOs.Users;
using GestioneDb.Models;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Security;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

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

        public async Task<Result<UserResponseDTO>> GetUserByIdAsync(int id)
        {
            var Response = await _context.Users.FindAsync(id);

            if (Response == null)
                return Result<UserResponseDTO>.Fail(ErrorCode.NotFound, "User not found");

            var user = new UserResponseDTO()
            {
                UserID = (int)Response.UserID,
                Username = Response.Username,
                CreatedAt = Response.CreatedAt
            };

            return Result<UserResponseDTO>.Ok(user);
        }

        public async Task<Result<RegisterResponseDTO>> CreateUserAsync(RegisterDTO Credentials, JwtService jwt)
        {
            if (Credentials == null)
                return Result<RegisterResponseDTO>.Fail(ErrorCode.BadRequest, "Credentials aren't valid");

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == Credentials.Username);

            if (existing != null)
                return Result<RegisterResponseDTO>.Fail(ErrorCode.BadRequest, "This username already exists");

            string HashedPassword, Salt;

            try
            {
                (HashedPassword, Salt) = HashingService.HashPassword(Credentials.Password);
            }
            catch (Exception)
            {
                return Result<RegisterResponseDTO>.Fail(ErrorCode.InternalServerError, "Internal server error during the password hashing");
            }

            var NewUser = new Models.User
            {
                Username = Credentials.Username,
                HashedPassword = HashedPassword,
                PasswordSalt = Salt,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            var tokenUser = new Models.User
            {
                UserID = NewUser.UserID,
                Username = Credentials.Username,
                HashedPassword = HashedPassword,
                PasswordSalt = Salt,
                CreatedAt = DateTime.Now
            };

            var token = jwt.GenerateToken(tokenUser);

            var response = new RegisterResponseDTO
            {
                UserID = (int)NewUser.UserID,
                Username = NewUser.Username,
                CreatedAt = NewUser.CreatedAt,
                Token = token
            };

            return Result<RegisterResponseDTO>.Ok(response);
        }

        public async Task<Result<bool>> UpdateUserByIdAsync(int id, UpdateUserDTO ModifiedUser)
        {
            if (ModifiedUser == null)
                return Result<bool>.Fail(ErrorCode.BadRequest, "The informations are null");

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return Result<bool>.Fail(ErrorCode.NotFound, "User not found");

            if (!string.IsNullOrEmpty(ModifiedUser.Username))
            {
                var conflict = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == ModifiedUser.Username && u.UserID != id);

                if (conflict != null)
                {
                    return Result<bool>.Fail(ErrorCode.BadRequest, "This username already exists");
                }

                user.Username = ModifiedUser.Username;
            }

            if (!string.IsNullOrEmpty(ModifiedUser.Password))
            {
                try
                {
                    (user.HashedPassword, user.PasswordSalt) = HashingService.HashPassword(ModifiedUser.Password);
                }
                catch
                {
                    return Result<bool>.Fail(ErrorCode.InternalServerError, "Internal server error during the password hashing");
                }
            }

            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> DeleteUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return Result<bool>.Fail(ErrorCode.BadRequest, "User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO credentials, JwtService jwt)
        {
            if (credentials == null)
                return Result<LoginResponseDTO>.Fail(ErrorCode.BadRequest, "Credential are null");

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == credentials.Username);

            if (user == null)
                return Result<LoginResponseDTO>.Fail(ErrorCode.NotFound, "User not found");

            bool ok = HashingService.VerifyPassword(
                credentials.Password,
                user.HashedPassword,
                user.PasswordSalt
            );

            if (!ok)
                return Result<LoginResponseDTO>.Fail(ErrorCode.Unauthorized, "Wrong password");

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

            return Result<LoginResponseDTO>.Ok(obj);
        }
    }
}
