namespace Libreria.DTOs.Users
{
    /// <summary>
    /// Represents the data required for a user login
    /// </summary>
    public class LoginDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
