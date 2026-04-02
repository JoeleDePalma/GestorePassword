using System.Security.Cryptography;

namespace Security
{
    public class SecurityServices
    {
        /// <summary>
        /// Generates a cryptographically secure random salt to be used in a password hashing process
        /// </summary>
        /// <param name="SaltSize">
        /// The size of the salt in bytes. Defaults to 16
        /// </param>
        /// <returns>
        /// A Base64-encoded string representing the generated salt
        /// </returns>
        public static string GenerateSalt(int SaltSize = 16)
        {
            var SaltBytes = new byte[SaltSize];

            using (var RandomNumber = RandomNumberGenerator.Create())
            {
                RandomNumber.GetBytes(SaltBytes);
            }

            return Convert.ToBase64String(SaltBytes);
        }
    }
}
