using GestioneDb.DTOs.Users;

public class LoginResponseDTO : UserResponseDTO
{
    public string Token { get; set; }
}
