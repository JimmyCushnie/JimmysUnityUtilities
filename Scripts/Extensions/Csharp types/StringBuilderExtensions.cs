using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JimmysUnityUtilities
{
    public static class StringBuilderExtensions
    {
        public static int IndexOf(this StringBuilder builder, char find, int startIndex = 0)
        {
            for (int i = startIndex; i < builder.Length; ++i)
            {
                if (builder[i] == find)
                    return i;
            }

            return -1;
        }

        public static int IndexOf(this StringBuilder builder, string find, int startIndex = 0)
        {
            int maxSearchLength = builder.Length - find.Length + 1;

            int findIndex;
            for (int i = startIndex; i < maxSearchLength; ++i)
            {
                if (builder[i] == find[0])
                {
                    findIndex = 1;
                    while (findIndex < find.Length && builder[i + findIndex] == find[findIndex])
                        findIndex++;

                    if (findIndex == find.Length)
                        return i;
                }
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
        {
            int index = builder.IndexOf(search, startIndex);

            if (index < 0)
                return builder;

            builder.Remove(index, search.Length);
            builder.Insert(index, replace);

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
        {
            foreach (var element in elements)
            {
                builder.Insert(index, element);
                index += element.Length;
            }

            return builder;
        }

        public static StringBuilder PrependChain(this StringBuilder builder, IEnumerable<string> elements) 
            => builder.InsertChain(0, elements);

        public static StringBuilder AppendChain(this StringBuilder builder, IEnumerable<string> elements) 
            => builder.InsertChain(builder.Length - 1, elements);


        public static StringBuilder InsertChain(this StringBuilder builder, int index, params string[] elements)
            => builder.InsertChain(index, (IEnumerable<string>)elements);

        public static StringBuilder PrependChain(this StringBuilder builder, params string[] elements) 
            => builder.InsertChain(0, elements);

        public static StringBuilder AppendChain(this StringBuilder builder, params string[] elements) 
            => builder.InsertChain(builder.Length - 1, elements);
    }
}