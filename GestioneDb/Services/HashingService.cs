using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;
using Security;

namespace Security
{
    public class HashingService
    {

        public static (string Hash, string Salt) HashPassword(string password, string Salt = null)
        {
            if (Salt == null)
                Salt = SecurityServices.GenerateSalt();

            var SaltBytes = Convert.FromBase64String(Salt);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = SaltBytes,
                DegreeOfParallelism = 4,
                MemorySize = 1024 * 64,
                Iterations = 4
            };

            var HashBytes = argon2.GetBytes(32);
            var HashString = Convert.ToBase64String(HashBytes);

            return (HashString, Salt);
        }

        public static bool VerifyPassword(string password, string Hash, string Salt)
        {
            var (NewHash, _) = HashPassword(password, Salt);

            return CryptographicOperations.FixedTimeEquals(
                Convert.FromBase64String(NewHash),
                Convert.FromBase64String(Hash)
                );
        }
    }
}