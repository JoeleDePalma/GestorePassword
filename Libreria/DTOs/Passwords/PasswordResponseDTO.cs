namespace Libreria.DTOs.Passwords
{
    /// <summary>
    /// Represents the password data returned to the client
    /// </summary>
    public class PasswordResponseDTO
    {
        public int Id { get; set; }
        public required string AppName { get; set; }
        public required string AppUsername { get; set; }
        public required string Password { get; set; }
        public string? KeySalt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
