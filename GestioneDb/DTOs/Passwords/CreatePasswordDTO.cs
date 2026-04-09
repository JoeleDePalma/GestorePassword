namespace GestioneDb.DTOs.Passwords
{
    /// <summary>
    /// Represents the data required to create a new password entry
    /// </summary>
    public class CreatePasswordDTO
    {
        public required string AppName { get; set; }
        public string? AppUsername { get; set; }
        public required string Password { get; set; }
        public required string MasterPassword { get; set; }
    }
}