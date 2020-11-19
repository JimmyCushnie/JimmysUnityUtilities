using System;

namespace JimmysUnityUtilities
{
    public static class CharExtensions
    {
        public static bool IsWhitespaceOrNonBreakingSpace(this char value)
            => Char.IsWhiteSpace(value) || value == '\u200B';
    }
}