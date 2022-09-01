using System;

namespace JimmysUnityUtilities
{
    public static class CharExtensions
    {
        public static bool IsWhitespaceOrNonBreakingSpace(this char value)
            => value.IsWhitespace() || value == '\u200B';

        public static bool EqualsCaseInsensitive(this char value, char other)
            => Char.ToUpperInvariant(value) == Char.ToUpperInvariant(other);


        public static bool IsDecimalDigit(this char value)
            => value >= '0' && value <= '9';


        public static bool IsDigit(this char value)
            => Char.IsDigit(value);

        public static bool IsLetter(this char value)
            => Char.IsLetter(value);

        public static bool IsWhitespace(this char value)
            => Char.IsWhiteSpace(value);

        public static bool IsUpper(this char value)
            => Char.IsUpper(value);

        public static bool IsLower(this char value)
            => Char.IsLower(value);

        public static bool IsPunctuation(this char value)
            => Char.IsPunctuation(value);

        public static bool IsLetterOrDigit(this char value)
            => Char.IsLetterOrDigit(value);

        public static bool IsControl(this char value)
            => Char.IsControl(value);

        public static bool IsNumber(this char value)
            => Char.IsNumber(value);

        public static bool IsSeparator(this char value)
            => Char.IsSeparator(value);

        public static bool IsSurrogate(this char value)
            => Char.IsSurrogate(value);

        public static bool IsSymbol(this char value)
            => Char.IsSymbol(value);
    }
}