using System.Text;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        public string UppercaseAlphanumericString(int length)
        {
            const string uppercaseAlphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var sb = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                char nextChar = uppercaseAlphanumericCharacters[this.Range(0, uppercaseAlphanumericCharacters.Length - 1)];
                sb.Append(nextChar);
            }

            return sb.ToString();
        }

        // todo more useful string methods
        // todo generate random strings of emojis lmao
    }
}
