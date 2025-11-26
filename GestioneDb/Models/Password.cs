namespace GestioneDb.Models
{
    public class Password
    {
        public int Id { get; set; }
        public string? App { get; set; }
        public byte[]? EncryptedPassword { get; set; }
    }
}
