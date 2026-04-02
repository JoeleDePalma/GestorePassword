namespace GestioneDb.DTOs.Passwords
{
    /// <summary>
    /// Represents the password data returned to the client
    /// </summary>
    public class CreatedPasswordDTO : PasswordResponseDTO
    {
        public int CredentialID { get; set; }
    }
}
