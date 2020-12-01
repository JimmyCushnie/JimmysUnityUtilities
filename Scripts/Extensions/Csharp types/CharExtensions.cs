using System;

namespace JimmysUnityUtilities
{
    public static class CharExtensions
    {
        public static bool IsWhitespace(this char value)
            => Char.IsWhiteSpace(value);

        public static bool IsWhitespaceOrNonBreakingSpace(this char value)
            => value.IsWhitespace() || value == '\u200B';

        public static bool EqualsCaseInsensitive(this char value, char other)
            => Char.ToUpperInvariant(value) == Char.ToUpperInvariant(other);
    }
}