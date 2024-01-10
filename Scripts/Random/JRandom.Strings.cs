using System.Text;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        private string GenerateRandomString(char[] sourceChars, int length)
        {
            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char nextChar = this.RandomElementOf(sourceChars);
                sb.Append(nextChar);
            }

            return sb.ToString();
        }


        private static readonly char[] UppercaseAlphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        public string UppercaseAlphanumericString(int length)
            => GenerateRandomString(UppercaseAlphanumericCharacters, length);

        private static readonly char[] LowercaseAlphanumericCharacters = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        public string LowercaseAlphanumericString(int length)
            => GenerateRandomString(LowercaseAlphanumericCharacters, length);

        private static readonly char[] MixedCaseAlphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        public string MixedCaseAlphanumericString(int length)
            => GenerateRandomString(MixedCaseAlphanumericCharacters, length);

        // todo more useful string methods
        // todo generate random strings of emojis lmao
    }
}
