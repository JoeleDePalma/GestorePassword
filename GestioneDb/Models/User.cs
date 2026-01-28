namespace GestioneDb.Models
{
    public class User
    {
        public int? UserID { get; set; }
        public string Username { get; set; }
        public byte[] HashedPassword { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
