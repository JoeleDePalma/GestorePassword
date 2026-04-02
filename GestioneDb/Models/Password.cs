using System.ComponentModel.DataAnnotations;

namespace GestioneDb.Models
{
    /// <summary>
    /// Represents a stored credential belonging to a specific user.
    /// </summary>
    public class Password
    {
        /// <summary>
        /// The unique identifier of the credential.
        /// </summary>
        [Key]
        public int CredentialID { get; set; }

        /// <summary>
        /// The ID of the user who owns this credential.
        /// </summary>
        public int UserID { get; set; }


        /// <summary>
        /// The name of the application or service associated with the credential.
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// The username used for the application or service.
        /// </summary>
        public string AppUsername { get; set; }


        /// <summary>
        /// The encrypted version of the stored password.
        /// </summary>
        public string EncryptedPassword { get; set; }

        /// <summary>
        /// The salt used to derive the encryption key for this password.
        /// </summary>
        public string KeySalt { get; set; }


        /// <summary>
        /// The UTC date and time when the credential was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The UTC date and time when the credential was last updated.
        /// </summary>
        public DateTime LastUpdateAt { get; set; }
    }
}