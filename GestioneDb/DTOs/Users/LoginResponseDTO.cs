namespace GestioneDb.DTOs.Users
{
    /// <summary>
    /// Represents the data returned to the client after a successful login
    /// </summary>
    public class LoginResponseDTO : UserResponseDTO
    {
        public string Token { get; set; }
    }
}