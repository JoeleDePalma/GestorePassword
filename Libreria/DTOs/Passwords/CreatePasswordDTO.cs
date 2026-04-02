namespace Libreria.DTOs.Passwords
{
    /// <summary>
    /// Represents the data required to create a new password entry
    /// </summary>
    public class CreatePasswordDTO
    {
        public string AppName { get; set; }
        public string? AppUsername { get; set; }
        public string Password { get; set; }
        public string MasterPassword { get; set; }
    }
}
