using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace JimmysUnityUtilities
{
    public static class CryptographyUtility
    {
        public static byte[] GetBinaryHash_SHA256(string source)
        {
            using (var hasher = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(source ?? string.Empty);
                return hasher.ComputeHash(bytes);
            }
        }


        public static string GetFileSHA1(string filePath)
        {
            using (var fileStream = File.OpenRead(filePath))
            using (var sha1 = new SHA1Managed())
            {
                byte[] hashBytes = sha1.ComputeHash(fileStream);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }


        public static void SaveTextToDiskEncrypted(string text, string filePath, byte[] encryptionKey, byte[] encryptionIV)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.IV = encryptionIV;

                using (var fileStream = File.OpenWrite(filePath))
                using (var encryptedStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (var encryptedWriter = new StreamWriter(encryptedStream, Encoding.UTF8))
                {
                    encryptedWriter.Write(text);
                }
            }
        }

        public static string LoadTextFromDiskEncrypted(string filePath, byte[] encryptionKey, byte[] encryptionIV)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = encryptionKey;
                aes.IV = encryptionIV;

                using (var fileStream = File.OpenRead(filePath))
                using (var encryptedStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (var encryptedReader = new StreamReader(encryptedStream, Encoding.UTF8))
                {
                    return encryptedReader.ReadToEnd();
                }
            }
        }


        public static byte[] GenerateCryptographicallySecureRandomBytes(int length)
        {
            using (var cryptoRng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[length];
                cryptoRng.GetBytes(buffer);
                return buffer;
            }
        }
    }
}