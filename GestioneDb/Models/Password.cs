namespace GestioneDb.Models
{
    public class Password
    {
        public int? CredentialID { get; set; }
        public int UserID { get; set; }

        public string AppName { get; set; }
        public string AppUsername { get; set; }

        public string EncryptedPassword { get; set; }
        public string IV { get; set; }
        public string SaltIV { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
