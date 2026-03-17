namespace GestioneDb.DTOs.Passwords
{
    public class CreatePasswordDTO
    {
        public string AppName { get; set; }
        public string? AppUsername { get; set; }
        public string Password { get; set; }
        public string MasterPassword { get; set; }
    }
}
