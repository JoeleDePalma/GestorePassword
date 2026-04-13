using System;

namespace GestorePassword.Core.Models
{
    public class UserInfo
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; }
    }
}
