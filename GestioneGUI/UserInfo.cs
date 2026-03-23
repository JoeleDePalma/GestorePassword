using System;
using System.Collections.Generic;
using System.Text;

namespace GestioneGUI
{
    public class UserInfo
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Token { get; set; }
    }
}
