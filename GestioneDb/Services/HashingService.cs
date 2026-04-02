using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Security
{
    public class HashingService
    {
        /// <summary>
        /// Hashes the provided password using the Argon2id algorithm, generating a new salt if none is provided
        /// </summary>
        /// <param name="password">The plaintext password to hash </param>
        /// <param name="salt">
        /// The Base64-encoded salt to use for hashing. If null, a new salt is generated
        /// </param>
        /// <returns>
        /// A tuple containing the Base64-encoded hash and the salt used during hashing
        /// </returns>
        public static (string hash, string salt) HashPassword(string password, string salt = null)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be null or empty.", nameof(password));

            if (salt == null)
                salt = SecurityServices.GenerateSalt();

            var saltBytes = Convert.FromBase64String(salt);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = saltBytes,
                DegreeOfParallelism = 4,
                MemorySize = 1024 * 64,
                Iterations = 4
            };

            var hashBytes = argon2.GetBytes(32);
            var hashString = Convert.ToBase64String(hashBytes);

            return (hashString, salt);
        }

        /// <summary>
        /// Verifies whether the provided password matches the stored hash by re‑hashing it
        /// with the same salt and comparing the results using a constant‑time comparison.
        /// </summary>
        /// <param name="password">The plaintext password to validate.</param>
        /// <param name="Hash">The Base64‑encoded hash stored in the database.</param>
        /// <param name="Salt">The Base64‑encoded salt used to generate the stored hash.</param>
        /// <returns>
        /// <c>true</c> if the password is valid; otherwise <c>false</c>.
        /// </returns>
        public static bool VerifyPassword(string password, string Hash, string Salt)
        {
            try
            {
                var (NewHash, _) = HashPassword(password, Salt);

                return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(NewHash),
                Convert.FromBase64String(Hash)
                );
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}