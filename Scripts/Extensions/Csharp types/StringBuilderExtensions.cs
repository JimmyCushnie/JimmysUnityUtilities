using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JimmysUnityUtilities
{
    public static class StringBuilderExtensions
    {
        public static int IndexOf(this StringBuilder builder, char find, int startIndex = 0, bool ignoreCase = false)
        {
            if (ignoreCase)
                find = Char.ToLowerInvariant(find);

            for (int i = startIndex; i < builder.Length; ++i)
            {
                char charAtIndex = builder[i];

                if (ignoreCase)
                    charAtIndex = Char.ToLowerInvariant(charAtIndex);

                if (charAtIndex == find)
                    return i;
            }

            return -1;
        }

        public static int IndexOf(this StringBuilder builder, string find, int startIndex = 0, bool ignoreCase = false)
        {
            int maxSearchLength = builder.Length - find.Length + 1;

            int findIndex;
            for (int i = startIndex; i < maxSearchLength; ++i)
            {
                char charAtIndex = GetCharInBuilder(i);

                if (charAtIndex == GetCharInFind(0))
                {
                    findIndex = 1;
                    while (findIndex < find.Length && GetCharInBuilder(i + findIndex) == GetCharInFind(findIndex))
                        findIndex++;

                    if (findIndex == find.Length)
                        return i;
                }
            }

            return -1;


            char GetCharInBuilder(int index)
            {
                if (ignoreCase)
                    return char.ToLowerInvariant(builder[index]);

                return builder[index];
            }
            char GetCharInFind(int index) // Ideally we would cache this somehow
            {
                if (ignoreCase)
                    return char.ToLowerInvariant(find[index]);

                return find[index];
            }
        }

        public static int IndexOfWhitespace(this StringBuilder builder, int startIndex = 0)
        {
            for (int i = startIndex; i < builder.Length; ++i)
            {
                if (Char.IsWhiteSpace(builder[i]))
                    return i;
            }

            return -1;
        }

        public static int CountInstancesOf(this StringBuilder builder, string find)
        {
            int count = 0;

            int nextIndex = 0;
            while (nextIndex > -1)
            {
                nextIndex = builder.IndexOf(find, nextIndex + find.Length);

                if (nextIndex > -1)
                    count++;
            }

            return count;
        }

        public static StringBuilder ReplaceFirst(this StringBuilder builder, string search, string replace, int startIndex = 0)
            => builder.ReplaceFirst(search, replace, out _, startIndex);

        public static StringBuilder ReplaceFirst(this StringBuilder builder, string search, string replace, out int indexOfReplacementStart, int startIndex = 0)
        {
            indexOfReplacementStart = builder.IndexOf(search, startIndex);

            if (indexOfReplacementStart < 0)
                return builder;

            builder.Remove(indexOfReplacementStart, search.Length);
            builder.Insert(indexOfReplacementStart, replace);

            return builder;
        }

        /// <summary>
        /// Replace any characters from a list with a given character.
        /// </summary>
        public static StringBuilder ReplaceAny(this StringBuilder builder, char[] search, char replace, int startIndex = 0)
        {
            for (int i = startIndex; i < builder.Length; i++)
            {
                foreach (var candidate in search)
                {
                    if (builder[i] == candidate)
                    {
                        builder[i] = replace;
                        break;
                    }
                }
            }

            return builder;
        }


        public static StringBuilder Prepend(this StringBuilder builder, string value)
            => builder.Insert(0, value);



        public static StringBuilder TrimStart(this StringBuilder builder, char remove = ' ')
        {
            while (builder.Length > 0 && builder[0] == remove)
                builder.Remove(startIndex: 0, length: 1);

            return builder;
        }

        public static StringBuilder TrimStart(this StringBuilder builder, params char[] remove)
        {
            while (builder.Length > 0 && remove.Contains(builder[0]))
                builder.Remove(startIndex: 0, length: 1);

            return builder;
        }


        public static StringBuilder TrimEnd(this StringBuilder builder, char remove = ' ')
        {
            while (builder.Length > 0 && builder[builder.Length - 1] == remove)
                builder.Remove(startIndex: builder.Length - 1, length: 1);

            return builder;
        }

        public static StringBuilder TrimEnd(this StringBuilder builder,  char[] remove)
        {
            while (builder.Length > 0 && remove.Contains(builder[builder.Length - 1]))
                builder.Remove(startIndex: builder.Length - 1, length: 1);

            return builder;
        }


        public static StringBuilder Trim(this StringBuilder builder, char remove = ' ')
        {
            builder.TrimEnd(remove);
            builder.TrimStart(remove);

            return builder;
        }

        public static StringBuilder Trim(this StringBuilder builder, params char[] remove)
        {
            builder.TrimEnd(remove);
            builder.TrimStart(remove);

            return builder;
        }


        public static StringBuilder RemoveCharactersAtStart(this StringBuilder builder, int characterCount)
        {
            if (characterCount > builder.Length)
                throw new ArgumentException($"{nameof(characterCount)} is greater than the length of the builder");

            if (characterCount < 0)
                throw new ArgumentException($"{nameof(characterCount)} cannot be less than 0");


            builder.Remove(startIndex: 0, length: characterCount);
            return builder;
        }
        public static StringBuilder RemoveCharactersAtEnd(this StringBuilder builder, int characterCount)
        {
            if (characterCount > builder.Length)
                throw new ArgumentException($"{nameof(characterCount)} is greater than the length of the builder");

            if (characterCount < 0)
                throw new ArgumentException($"{nameof(characterCount)} cannot be less than 0");


            builder.Remove(startIndex: builder.Length - characterCount, length: characterCount);
            return builder;
        }


        public static bool StartsWith(this StringBuilder builder, char character)
            => builder.Length > 0 && builder[0] == character;

        public static bool StartsWith(this StringBuilder builder, string substring)
        {
            if (builder.Length < substring.Length)
                return false;

            for (int i = 0; i < substring.Length; i++)
            {
                if (builder[i] != substring[i])
                    return false;
            }

            return true;
        }


        public static string Snip(this StringBuilder builder, int startIndex, int endIndex)
        {
            if (startIndex < 0)
                throw new ArgumentException($"{nameof(startIndex)} must not be less than 0");

            if (endIndex < 0)
                throw new ArgumentException($"{nameof(endIndex)} must not be less than 0");

            if (endIndex < startIndex)
                throw new ArgumentException($"{nameof(endIndex)} must not be less than {nameof(startIndex)}");

            if (startIndex >= builder.Length)
                throw new ArgumentOutOfRangeException($"{nameof(startIndex)} is outside the range of the string!");

            if (endIndex >= builder.Length)
                throw new ArgumentOutOfRangeException($"{nameof(endIndex)} is outside the range of the string!");


            int length = endIndex - startIndex + 1;
            return builder.ToString(startIndex, length);
        }


        public static StringBuilder InsertChain(this StringBuilder builder, int index, IEnumerable<string> elements)
            => InsertChain(builder, index, out var _, elements);

        public static StringBuilder InsertChain(this StringBuilder builder, int index, out int insertionEndIndex, IEnumerable<string> elements)
        {
            foreach (var element in elements)
            {
                builder.Insert(index, element);
                index += element.Length;
            }

            insertionEndIndex = index;
            return builder;
        }

        public static StringBuilder PrependChain(this StringBuilder builder, IEnumerable<string> elements) 
            => builder.InsertChain(0, elements);

        public static StringBuilder PrependChain(this StringBuilder builder, out int insertionEndIndex, IEnumerable<string> elements) 
            => builder.InsertChain(0, out insertionEndIndex, elements);

        public static StringBuilder AppendChain(this StringBuilder builder, IEnumerable<string> elements) 
            => builder.InsertChain(builder.Length, elements);


        public static StringBuilder InsertChain(this StringBuilder builder, int index, params string[] elements)
            => builder.InsertChain(index, (IEnumerable<string>)elements);

        public static StringBuilder InsertChain(this StringBuilder builder, int index, out int insertionEndIndex, params string[] elements)
            => builder.InsertChain(index, out insertionEndIndex, (IEnumerable<string>)elements);

        public static StringBuilder PrependChain(this StringBuilder builder, params string[] elements) 
            => builder.InsertChain(0, elements);

        public static StringBuilder PrependChain(this StringBuilder builder, out int insertionEndIndex, params string[] elements) 
            => builder.InsertChain(0, out insertionEndIndex, elements);

        public static StringBuilder AppendChain(this StringBuilder builder, params string[] elements) 
            => builder.InsertChain(builder.Length, elements);



        public static bool IsEmpty(this StringBuilder builder)
            => builder.Length == 0;

        public static bool IsNotEmpty(this StringBuilder builder)
            => builder.Length > 0;


        public static bool IsEmptyOrWhitespace(this StringBuilder builder)
        {
            if (builder.IsEmpty())
                return true;

            for (int i = 0; i < builder.Length; i++)
            {
                if (!Char.IsWhiteSpace(builder[i]))
                    return false;
            }

            return true;
        }


        public static bool IsExclusively(this StringBuilder builder, char target)
        {
            if (builder.IsEmpty())
                return false;

            for (int i = 0; i < builder.Length; i++)
            {
                if (builder[i] != target)
                    return false;
            }

            return true;
        }



        public static int FirstIndex(this StringBuilder builder)
        {
            if (builder.Length == 0)
                throw new Exception("StringBuilder is empty, and therefore has no first index");

            return 0;
        }
        public static int LastIndex(this StringBuilder builder)
        {
            if (builder.Length == 0)
                throw new Exception("StringBuilder is empty, and therefore has no last index");

            return builder.Length - 1;
        }
    }
}