using System;

namespace ClassesLibrary
{
    public class User
    {
        public int? UserID { get; set; }
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
