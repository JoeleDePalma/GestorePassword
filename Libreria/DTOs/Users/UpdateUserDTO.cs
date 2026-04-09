namespace Libreria.DTOs.Users
{
    /// <summary>
    /// Represents the data that can be updated for an existing user
    /// </summary>
    public class UpdateUserDTO
    {
        public string? Username { get; set; }
        public string? NewPassword { get; set; }
        public required string CurrentPassword { get; set; }
    }
}
