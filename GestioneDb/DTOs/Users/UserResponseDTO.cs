namespace GestioneDb.DTOs.Users
{
    /// <summary>
    /// Represents the user data returned to the client
    /// </summary>
    public class UserResponseDTO
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
