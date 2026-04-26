using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace GestorePassword.Core.Services
{
    public class PasswordGenerator
    {
        public static string GeneratePassword(int length = 16)
        {
            if (length < 4)
                throw new ArgumentException("La password deve essere lunga almeno 4 caratteri.");

            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string special = "!@#$%^&*()";

            string allChars = lower + upper + digits + special;

            char[] result = new char[length];

            using (var rng = RandomNumberGenerator.Create())
            {
                result[0] = lower[GetRandomIndex(rng, lower.Length)];
                result[1] = upper[GetRandomIndex(rng, upper.Length)];
                result[2] = digits[GetRandomIndex(rng, digits.Length)];
                result[3] = special[GetRandomIndex(rng, special.Length)];

                for (int i = 4; i < length; i++)
                    result[i] = allChars[GetRandomIndex(rng, allChars.Length)];

                Shuffle(result, rng);
            }

            return new string(result);
        }

        private static int GetRandomIndex(RandomNumberGenerator rng, int max)
        {
            byte[] buffer = new byte[1];
            rng.GetBytes(buffer);
            return buffer[0] % max;
        }

        private static void Shuffle(char[] array, RandomNumberGenerator rng)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = GetRandomIndex(rng, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

    }
}
