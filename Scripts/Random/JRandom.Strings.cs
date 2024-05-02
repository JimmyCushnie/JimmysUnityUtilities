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


        private static readonly char[] UppercaseAlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        public string UppercaseAlphanumericString(int length)
            => GenerateRandomString(UppercaseAlphanumericChars, length);

        private static readonly char[] LowercaseAlphanumericChars = "abcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        public string LowercaseAlphanumericString(int length)
            => GenerateRandomString(LowercaseAlphanumericChars, length);

        private static readonly char[] MixedCaseAlphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        public string MixedCaseAlphanumericString(int length)
            => GenerateRandomString(MixedCaseAlphanumericChars, length);

        private static readonly char[] UppercaseLettersChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public string UppercaseLetters(int length)
            => GenerateRandomString(UppercaseLettersChars, length);

        private static readonly char[] LowercaseLettersChars = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
        public string LowercaseLetters(int length)
            => GenerateRandomString(LowercaseLettersChars, length);

        private static readonly char[] MixedCaseLettersChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
        public string MixedCaseLetters(int length)
            => GenerateRandomString(MixedCaseLettersChars, length);

        private static readonly char[] FaceEmojiChars = "😀😁😂🤣😃😄😅😆😉😊😋😎😍😘🥰😗😙😚🙂🤗🤩🤔🤨😐😑😶🙄😏😣😥😮🤐😯😪😫🥱😴😌😛😜😝🤤😒😓😔😕🙃🤑😲☹🙁😖😞😟😤😢😭😦😧😨😩🤯😬😰😱🥵🥶😳🤪😵🥴😠😡🤬😷🤒🤕🤢🤮🤧😇🥳🥺🤡🤠🤥🤫🤭🧐🤓".ToCharArray();
        public string FaceEmojis(int length)
            => GenerateRandomString(FaceEmojiChars, length);
    }
}
