using System;
using System.Linq;
using System.Text;

namespace JimmysUnityUtilities
{
    public static class StringBuilderExtensions
    {
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

        public static void ReplaceFirst(this StringBuilder builder, string search, string replace, int startIndex = 0)
        {
            int index = builder.IndexOf(search, startIndex);

            if (index < 0)
                return;

            builder.Remove(index, search.Length);
            builder.Insert(index, replace);
        }

        public static void Prepend(this StringBuilder builder, string value)
            => builder.Insert(0, value);



        public static void TrimStart(this StringBuilder builder, char remove = ' ')
        {
            while (builder.Length > 0 && builder[0] == remove)
                builder.Remove(startIndex: 0, length: 1);
        }

        public static void TrimStart(this StringBuilder builder, params char[] remove)
        {
            while (builder.Length > 0 && remove.Contains(builder[0]))
                builder.Remove(startIndex: 0, length: 1);
        }


        public static void TrimEnd(this StringBuilder builder, char remove = ' ')
        {
            while (builder.Length > 0 && builder[builder.Length - 1] == remove)
                builder.Remove(startIndex: builder.Length - 1, length: 1);
        }

        public static void TrimEnd(this StringBuilder builder,  char[] remove)
        {
            while (builder.Length > 0 && remove.Contains(builder[builder.Length - 1]))
                builder.Remove(startIndex: builder.Length - 1, length: 1);
        }


        public static void Trim(this StringBuilder builder, char remove = ' ')
        {
            builder.TrimEnd(remove);
            builder.TrimStart(remove);
        }

        public static void Trim(this StringBuilder builder, params char[] remove)
        {
            builder.TrimEnd(remove);
            builder.TrimStart(remove);
        }
    }
}