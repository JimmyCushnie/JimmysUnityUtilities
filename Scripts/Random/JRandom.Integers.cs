using System;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        /// <summary> Gets a random value in the range [<see cref="sbyte.MinValue"/>, <see cref="sbyte.MaxValue"/>]. </summary>
        public sbyte SByte()
        {
            unchecked
            {
                return (sbyte)GetSourceRandom64Bits();
            }
        }

        /// <summary> Gets a random value in the range [<see cref="byte.MinValue"/>, <see cref="byte.MaxValue"/>]. </summary>
        public byte Byte()
        {
            unchecked
            {
                return (byte)GetSourceRandom64Bits();
            }
        }

        /// <summary> Gets a random value in the range [<see cref="short.MinValue"/>, <see cref="short.MaxValue"/>]. </summary>
        public short Int16()
        {
            unchecked
            {
                return (short)GetSourceRandom64Bits();
            }
        }

        /// <summary> Gets a random value in the range [<see cref="ushort.MinValue"/>, <see cref="ushort.MaxValue"/>]. </summary>
        public ushort UInt16()
        {
            unchecked
            {
                return (ushort)GetSourceRandom64Bits();
            }
        }

        /// <summary> Gets a random value in the range [<see cref="int.MinValue"/>, <see cref="int.MaxValue"/>]. </summary>
        public int Int32()
        {
            unchecked
            {
                return (int)GetSourceRandom64Bits();
            }
        }

        /// <summary> Gets a random value in the range [<see cref="uint.MinValue"/>, <see cref="uint.MaxValue"/>]. </summary>
        public uint UInt32()
        {
            unchecked
            {
                return (uint)GetSourceRandom64Bits();
            }
        }

        /// <summary> Gets a random value in the range [<see cref="long.MinValue"/>, <see cref="long.MaxValue"/>]. </summary>
        public long Int64()
        {
            unchecked
            {
                return (long)GetSourceRandom64Bits();
            }
        }

        /// <summary> Gets a random value in the range [<see cref="ulong.MinValue"/>, <see cref="ulong.MaxValue"/>]. </summary>
        public ulong UInt64()
        {
            return GetSourceRandom64Bits();
        }






        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public ulong Range(ulong minInclusive, ulong maxInclusive)
        {
            if (minInclusive > maxInclusive)
                throw new ArgumentException($"{nameof(minInclusive)} cannot be greater than {nameof(maxInclusive)}");

            if (minInclusive == maxInclusive)
                return minInclusive;


            // Narrow down to the smallest range [0, 2^bits] that contains maxValue.
            // Then repeatedly generate a value in that outer range until we get one within the inner range.
            // The worst possible odds are 50% at generating a value in the range, and generating a new value is very fast,
            // so there shouldn't be any performance concerns with this method.
            ulong inclusiveRange = maxInclusive - minInclusive;
            int maxBits = BitOperations.Log2(inclusiveRange) + 1;
            while (true)
            {
                // Discard all the bits except enough to fill our power-of-two quota
                ulong result = GetSourceRandom64Bits() >> (sizeof(ulong) * 8 - maxBits);

                if (result <= inclusiveRange)
                    return result + minInclusive;
            }
        }

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public long Range(long minInclusive, long maxInclusive)
        {
            if (minInclusive > maxInclusive)
                throw new ArgumentException($"{nameof(minInclusive)} cannot be greater than {nameof(maxInclusive)}");

            if (minInclusive == maxInclusive)
                return minInclusive;


            ulong inclusiveRange = (ulong)(maxInclusive - minInclusive);
            int maxBits = BitOperations.Log2(inclusiveRange) + 1;
            while (true)
            {
                ulong result = GetSourceRandom64Bits() >> (sizeof(ulong) * 8 - maxBits);

                if (result <= inclusiveRange)
                    return (long)result + minInclusive;
            }
        }


        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public sbyte Range(sbyte minInclusive, sbyte maxInclusive)
            => (sbyte)Range((long)minInclusive, (long)maxInclusive);

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public byte Range(byte minInclusive, byte maxInclusive)
            => (byte)Range((ulong)minInclusive, (ulong)maxInclusive);

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public short Range(short minInclusive, short maxInclusive)
            => (short)Range((long)minInclusive, (long)maxInclusive);

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public ushort Range(ushort minInclusive, ushort maxInclusive)
            => (ushort)Range((ulong)minInclusive, (ulong)maxInclusive);

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public int Range(int minInclusive, int maxInclusive)
            => (int)Range((long)minInclusive, (long)maxInclusive);

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public uint Range(uint minInclusive, uint maxInclusive)
            => (uint)Range((ulong)minInclusive, (ulong)maxInclusive);
    }
}
