namespace GestioneDb.DTOs.Users
{
    /// <summary>
    /// Represents the data returned to the client after a successful registration
    /// </summary>
    public class RegisterResponseDTO : UserResponseDTO
    {
        public string? Token { get; set; }
    }
}
