using System.Security.Cryptography;

namespace Security
{
    public class SecurityServices
    {
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
