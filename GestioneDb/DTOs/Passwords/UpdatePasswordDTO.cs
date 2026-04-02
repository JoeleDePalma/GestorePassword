namespace GestioneDb.DTOs.Passwords
{
    /// <summary>
    /// Represents the data that can be updated for an existing password entry
    /// </summary>
    public class UpdatePasswordDTO
    {
        public string? AppName { get; set; }
        public string? AppUsername { get; set; }
        public string? Password { get; set; }
        public string MasterPassword { get; set; }
    }
}