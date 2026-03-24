namespace GestioneDb.DTOs.Passwords
{
    public class PasswordResponseDTO
    {
        public int? Id { get; set; }
        public string AppName { get; set; }
        public string AppUsername { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
