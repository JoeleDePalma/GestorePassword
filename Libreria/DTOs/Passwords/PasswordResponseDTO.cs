namespace Libreria.DTOs.Passwords
{
    public class PasswordResponseDTO
    {
        public string AppName { get; set; }
        public string AppUsername { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
