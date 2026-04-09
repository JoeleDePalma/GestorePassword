namespace Libreria.DTOs.Users
{
    /// <summary>
    /// Represents the data required to register a new user
    /// </summary>
    public class RegisterDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
