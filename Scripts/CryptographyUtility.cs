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
    }
}