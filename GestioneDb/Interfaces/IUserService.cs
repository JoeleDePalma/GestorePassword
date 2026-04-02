using GestioneDb.DTOs.Users;
using GestioneDb.Models;
using Microsoft.AspNetCore.Mvc;
using GestioneDb.Services.Common;

namespace GestioneDb.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Retrieves the data of the user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a <see cref="UserResponseDTO"/> object
        /// if the operation succeeds, or an error result if the user does not exist.
        /// </returns>
        Task<Result<UserResponseDTO>> GetUserByIdAsync(int id);


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
        Task<Result<RegisterResponseDTO>> CreateUserAsync(RegisterDTO Credentials, JwtService jwt);

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
        Task<Result<bool>> UpdateUserByIdAsync(int id, UpdateUserDTO ModifiedUser);


        /// <summary>
        /// Deletes the specified user
        /// </summary>
        /// <param name="id">The ID of the user to delete </param>
        /// <returns>
        /// A <see cref="Result{T}"/> containing a boolean value indicating whether the deletion was successful,
        /// or an error result if the user does not exist
        /// </returns>
        Task<Result<bool>> DeleteUserByIdAsync(int id);


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
        Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO credentials, JwtService jwt);

    }
}
