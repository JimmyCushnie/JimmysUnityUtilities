using System;

namespace JimmysUnityUtilities
{
    public static class NumberUtilities
    {
        private static readonly (int numericValue, string representation)[] RomanNumeralsLeadingLettersDescending = new[]
        {
            (1000, "M"),
            (900, "CM"),
            (500, "D"),
            (400, "CD"),
            (100, "C"),
            (90, "XC"),
            (50, "L"),
            (40, "XL"),
            (10, "X"),
            (9, "IX"),
            (5, "V"),
            (4, "IV"),
            (1, "I")
        };

        public static string ConvertToRomanNumeral(int number)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException($"{nameof(number)} must be above 0");

            string romanNumeral = "";
            int remainder = number;
            foreach (var letter in RomanNumeralsLeadingLettersDescending)
            {
                while (remainder >= letter.numericValue)
                {
                    romanNumeral += letter.representation;
                    remainder -= letter.numericValue;
                }
            }

            return romanNumeral;
        }
    }
}