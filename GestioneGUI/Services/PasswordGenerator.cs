using System.Security.Cryptography;

namespace Services
{
    public static class PasswordGenerator
    {
        public static string GenerateSecurePassword(int length = 14)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()";
            char[] result = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[length];
                rng.GetBytes(buffer);

                for (int i = 0; i < length; i++)
                {
                    int index = buffer[i] % chars.Length;
                    result[i] = chars[index];
                }
            }

            return new string(result);
        }
    }
}