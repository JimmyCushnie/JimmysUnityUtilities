
// The version of .NET used by Unity doesn't have the BitOperations class. I need some methods from it for the JRandom stuff,
// so I provided implementations of them here, as fast as I can get them.
// Todo: once Unity finally ditches mono, use the much faster native .NET BitOperations.
// -Jimmy

namespace JimmysUnityUtilities.Random
{
    internal static class BitOperations
    {
        //https://stackoverflow.com/a/58496974
        //https://stackoverflow.com/a/10439333
        //https://github.com/SunsetQuest/Fast-Integer-Log2/blob/master/BenchmarkLeading0Count/Program.cs#L583
        //fuck
        public static int Log2(ulong x)
        {
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x -= x >> 1 & 0x55555555;
            x = (x >> 2 & 0x33333333) + (x & 0x33333333);
            x = (x >> 4) + x & 0x0f0f0f0f;
            x += x >> 8;
            x += x >> 16;
            return (int)((x & 0x0000003f) - 1);
        }

        public static ulong RotateLeft(ulong value, int offset)
            => (value << offset) | (value >> (64 - offset));
    }
}