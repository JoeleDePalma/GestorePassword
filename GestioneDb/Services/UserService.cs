using GestioneDb.Data;
using GestioneDb.DTOs.Users;
using GestioneDb.Models;
using GestioneDb.Services.Common;
using GestioneDb.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<Result<UserResponseDTO>> GetUserByIdAsync(int id)
        {
            var Response = await _context.Users.FindAsync(id);

            if (Response == null)
                return Result<UserResponseDTO>.Fail(ErrorCode.NotFound);

            var user = new UserResponseDTO()
            {
                UserID = Response.UserID,
                Username = Response.Username,
                CreatedAt = Response.CreatedAt
            };

            return Result<UserResponseDTO>.Ok(user);
        }

        public async Task<Result<UserResponseDTO>> CreateUserAsync(RegisterDTO Credentials)
        {
            if (Credentials == null)
                return Result<UserResponseDTO>.Fail(ErrorCode.BadRequest);

            var existing = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == Credentials.Username);

            if (existing != null)
                return Result<UserResponseDTO>.Fail(ErrorCode.BadRequest);

            string HashedPassword, Salt;

            try
            {
                (HashedPassword, Salt) = HashingService.HashPassword(Credentials.Password);
            }
            catch (Exception)
            {
                return Result<UserResponseDTO>.Fail(ErrorCode.InternalServerError);
            }

            var NewUser = new User
            {
                Username = Credentials.Username,
                HashedPassword = HashedPassword,
                PasswordSalt = Salt,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            var response = new UserResponseDTO
            {
                UserID = NewUser.UserID,
                Username = NewUser.Username,
                CreatedAt = NewUser.CreatedAt
            };

            return Result<UserResponseDTO>.Ok(response);
        }

        public async Task<Result<bool>> UpdateUserByIdAsync(int id, UpdateUserDTO ModifiedUser)
        {
            if (ModifiedUser == null)
                return Result<bool>.Fail(ErrorCode.BadRequest);

            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return Result<bool>.Fail(ErrorCode.NotFound);

            if (!string.IsNullOrEmpty(ModifiedUser.Username))
            {
                var conflict = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == ModifiedUser.Username && u.UserID != id);

                if (conflict != null)
                    return Result<bool>.Fail(ErrorCode.BadRequest);

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
                    return Result<bool>.Fail(ErrorCode.InternalServerError);
                }
            }

            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<Result<bool>> DeleteUserByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return Result<bool>.Fail(ErrorCode.BadRequest);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }

        public async Task<Result<string>> LoginAsync(LoginDTO credentials, JwtService jwt)
        {
            if (credentials == null)
                return Result<string>.Fail(ErrorCode.BadRequest);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == credentials.Username);

            if (user == null)
                return Result<string>.Fail(ErrorCode.Unauthorized);

            bool ok = HashingService.VerifyPassword(
                credentials.Password,
                user.HashedPassword,
                user.PasswordSalt
            );

            if (!ok)
                return Result<string>.Fail(ErrorCode.Unauthorized);

            var tokenUser = new User
            {
                UserID = user.UserID,
                Username = user.Username,
                HashedPassword = user.HashedPassword,
                PasswordSalt = user.PasswordSalt,
                CreatedAt = user.CreatedAt
            };

            var token = jwt.GenerateToken(tokenUser);

            return Result<string>.Ok(token);
        }
    }
}
