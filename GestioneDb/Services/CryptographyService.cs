using System.Text;
using System.Security.Cryptography;
using Konscious.Security.Cryptography;

namespace Security
{
    public class CryptographyService
    {
        /// <summary>
        /// Derives a cryptographic encryption key from the specified password using the
        /// Argon2id key derivation function. Generates a new salt if none is provided
        /// </summary>
        /// <param name="password">The plaintext password used to derive the key </param>
        /// <param name="salt">
        /// The Base64‑encoded salt used for key derivation. If null, a new salt is generated
        /// </param>
        /// <returns>
        /// A byte array containing the derived encryption key
        /// </returns>
        public static byte[] DeriveKey(string password, string salt = null)
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

        /// <summary>
        /// Encrypts the specified plaintext using AES‑CBC with PKCS7 padding and the
        /// provided encryption key. A new IV is generated for each encryption
        /// </summary>
        /// <param name="text">The plaintext string to encrypt </param>
        /// <param name="key">The encryption key as a byte array </param>
        /// <returns>
        /// A Base64‑encoded string containing the IV followed by the encrypted ciphertext
        /// </returns>
        public static string Encrypt(string text, byte[] key)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            aes.GenerateIV();
            var IV = aes.IV;

            var textBytes = Encoding.UTF8.GetBytes(text);

            var Encryptor = aes.CreateEncryptor(key, IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, Encryptor, CryptoStreamMode.Write))
            {
                cs.Write(textBytes, 0, textBytes.Length);
                cs.FlushFinalBlock();
            }

            var CipherBytes = ms.ToArray();

            var Result = new byte[IV.Length + CipherBytes.Length];

            Buffer.BlockCopy(IV, 0, Result, 0, IV.Length);
            Buffer.BlockCopy(CipherBytes, 0, Result, IV.Length, CipherBytes.Length);

            return Convert.ToBase64String(Result);
        }

        /// <summary>
        /// Decrypts a ciphertext previously encrypted with AES‑CBC and PKCS7 padding,
        /// extracting the IV from the beginning of the provided Base64‑encoded input.
        /// </summary>
        /// <param name="cipherTextBase64">
        /// A Base64‑encoded string containing the IV followed by the encrypted ciphertext.
        /// </param>
        /// <param name="key">The encryption key as a byte array.</param>
        /// <returns>
        /// The decrypted plaintext string.
        /// </returns>
        public static string Decrypt(string cipherTextBase64, byte[] key)
        {
            var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var fullCipher = Convert.FromBase64String(cipherTextBase64);

            var IVSize = aes.BlockSize / 8;

            var IV = new byte[IVSize];
            var cipherBytes = new byte[fullCipher.Length - IVSize];

            Buffer.BlockCopy(fullCipher, 0, IV, 0, IVSize);
            Buffer.BlockCopy(fullCipher, IVSize, cipherBytes, 0, cipherBytes.Length);

            aes.IV = IV;

            var Decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, Decryptor, CryptoStreamMode.Write))
            {
                cs.Write(cipherBytes, 0, cipherBytes.Length);
                cs.FlushFinalBlock();
            }

            var textBytes = ms.ToArray();

            return Encoding.UTF8.GetString(textBytes);
        }
    }
}
