namespace Libreria.DTOs.Passwords
{
    public class UpdatePasswordDTO
    {
        public string? AppName { get; set; }
        public string? AppUsername { get; set; }
        public string? Password { get; set; }
        public string MasterPassword { get; set; }
    }
}
