using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace JimmysUnityUtilities
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string value)
            => string.IsNullOrEmpty(value);

        public static bool IsNullOrWhiteSpace(this string value)
            => string.IsNullOrWhiteSpace(value);

        public static bool IsNotNullAndNotEmpty(this string value)
            => !string.IsNullOrEmpty(value);

        public static bool IsNotNullAndNotWhiteSpace(this string value)
            => !string.IsNullOrWhiteSpace(value);


        public static int CountInstancesOf(this string source, string substring)
        {
            int removedInstancesLength = source.Replace(substring, string.Empty).Length;
            return (source.Length - removedInstancesLength) / substring.Length;
        }

        public static int CountInstancesOf(this string source, char @char)
        {
            int removedInstancesLength = source.Replace(@char.ToString(), string.Empty).Length;
            return source.Length - removedInstancesLength;
        }

        /// <summary>
        /// Like <see cref="string.Replace(string, string)"/> but it only replaces the first instance
        /// </summary>
        public static string ReplaceFirst(this string text, string search, string replace, int startIndex = 0)
        {
            int index = text.IndexOf(search, startIndex);

            if (index < 0)
                return text;

            return text.Substring(0, index) + replace + text.Substring(index + search.Length);
        }

        /// <summary>
        /// Replace any characters from a list with a given character.
        /// </summary>
        public static string ReplaceAny(this string text, char[] search, char replace, int startIndex = 0)
        {
            var builder = new StringBuilder(text);
            builder.ReplaceAny(search, replace, startIndex);
            return builder.ToString();
        }

        /// <summary>
        /// Get a section of text
        /// </summary>
        public static string Snip(this string text, int startIndex, int endIndex)
        {
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(startIndex)} must not be less than 0");

            if (endIndex < 0)
                throw new ArgumentException($"{nameof(endIndex)} must not be less than 0");

            if (endIndex < startIndex)
                throw new ArgumentException($"{nameof(endIndex)} must not be less than {nameof(startIndex)}");

            if (startIndex >= text.Length)
                throw new ArgumentOutOfRangeException($"{nameof(startIndex)} is outside the range of the string!");

            if (endIndex >= text.Length)
                throw new ArgumentOutOfRangeException($"{nameof(endIndex)} is outside the range of the string!");


            int length = endIndex - startIndex + 1;
            return text.Substring(startIndex, length);
        }


        public static string[] Split(this string text, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return text.Split(new string[] { separator }, options);
        }

        public static string[] Split(this string text, string[] separators, StringSplitOptions options = StringSplitOptions.None)
        {
            return text.Split(separators, options);
        }


        public static string[] SplitIntoLines(this string s)
            => s.Replace("\r\n", "\n").Split('\n'); // fuck windows line endings. WHY are they still used.

        public static IEnumerator<string> ReadLineByLine(this string s)
        {
            using (StringReader sr = new StringReader(s))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}