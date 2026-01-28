using System;

namespace ClassesLibrary
{
    public class Password
    {
        public int? CredentialID { get; set; }
        public int UserID { get; set; }

        public string App { get; set; }
        public string AppUsername { get; set; }

        public byte[] EncryptedPassword { get; set; }
        public byte[] IV { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}