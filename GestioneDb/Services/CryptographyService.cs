using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Security
{
    public class CryptographyService
    {
        private static byte[] DeriveKey(string password, string salt = null)
        {
            if (salt == null)
                salt = SecurityServices.GenerateSalt();

            var SaltBytes = Convert.FromBase64String(salt);

            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = SaltBytes,
                DegreeOfParallelism = 8,
                MemorySize = 1024 * 64,
                Iterations = 4
            };

            byte[] Key = argon2.GetBytes(32);

            return Key;
        }

        public static string Encrypt(string Text, byte[] key)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            aes.GenerateIV();
            var IV = aes.IV;

            var TextBytes = Encoding.UTF8.GetBytes(Text);

            var Encryptor = aes.CreateEncryptor(key, IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, Encryptor, CryptoStreamMode.Write))
            {
                cs.Write(TextBytes, 0, TextBytes.Length);
                cs.FlushFinalBlock();
            }

            var CipherBytes = ms.ToArray();

            var Result = new byte[IV.Length + CipherBytes.Length];

            Buffer.BlockCopy(IV, 0, Result, 0, IV.Length);
            Buffer.BlockCopy(CipherBytes, 0, Result, IV.Length, CipherBytes.Length);

            return Convert.ToBase64String(Result);
        }

        public static string Decrypt(string CipherTextBase64, byte[] key)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var FullCipher = Convert.FromBase64String(CipherTextBase64);

            var IVSize = aes.BlockSize / 8;

            var IV = new byte[IVSize];
            var CipherBytes = new byte[FullCipher.Length - IVSize];

            Buffer.BlockCopy(FullCipher, 0, IV, 0, IVSize);
            Buffer.BlockCopy(FullCipher, IVSize, CipherBytes, 0, CipherBytes.Length);

            aes.IV = IV;

            var Decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, Decryptor, CryptoStreamMode.Write))
            {
                cs.Write(CipherBytes, 0, CipherBytes.Length);
                cs.FlushFinalBlock();
            }

            var TextBytes = ms.ToArray();

            return Encoding.UTF8.GetString(TextBytes);
        }
    }
}
