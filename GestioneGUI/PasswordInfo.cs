using System;
using System.Collections.Generic;
using System.Text;

namespace GestioneGUI
{
    public class PasswordInfo
    {
        public int Id { get; set; }
        public string App { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdateAt { get; set; }
    }
}
