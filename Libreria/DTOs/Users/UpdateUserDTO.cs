namespace Libreria.DTOs.Users
{
    /// <summary>
    /// Represents the data that can be updated for an existing user
    /// </summary>
    public class UpdateUserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
