namespace GestioneDb.Models
{
    public class Password
    {
        public int? CredentialID { get; set; }
        public int UserID { get; set; }

        public string AppName { get; set; }
        public string AppUsername { get; set; }

        public byte[] EncryptedPassword { get; set; }
        public byte[] IV { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
