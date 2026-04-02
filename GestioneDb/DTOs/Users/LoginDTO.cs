namespace GestioneDb.DTOs.Users
{
    /// <summary>
    /// Represents the data required for a user login
    /// </summary>
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}