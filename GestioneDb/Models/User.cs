namespace GestioneDb.Models
{
    /// <summary>
    /// Represents an application user stored in the database.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public int? UserID { get; set; }


        /// <summary>
        /// The username chosen by the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The hashed version of the user's password.
        /// </summary>
        public string HashedPassword { get; set; }


        /// <summary>
        /// The salt used during the password hashing process.
        /// </summary>
        public string PasswordSalt { get; set; }


        /// <summary>
        /// The UTC date and time when the user was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}