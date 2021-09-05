using System.Runtime.CompilerServices;

namespace JimmysUnityUtilities.Random
{
    /// <summary>
    /// Generates random values, in chunks of 64 bits. Does so extremely fast.
    /// </summary>
    public sealed class VeryFastRandomValueGenerator
    {
        // Implements the xoshiro256** algorithm (https://prng.di.unimi.it/)
        // Based on the code for unseeded instances of System.Random from the .NET 6 runtime

        // This is faster than any System.Random before .NET 6, and faster than seeded System.Random in .NET 6+.



        private ulong _s0, _s1, _s2, _s3;

        /// <summary>
        /// Creates a <see cref="VeryFastRandomValueGenerator"/> with a 64-bit seed.
        /// </summary>
        public VeryFastRandomValueGenerator(ulong seed)
        {
            ulong x = seed;

            do
            {
                _s0 = GetNextSplitMix64RandomValue();
                _s1 = GetNextSplitMix64RandomValue();
                _s2 = GetNextSplitMix64RandomValue();
                _s3 = GetNextSplitMix64RandomValue();
            }
            // At least one of the starting state values must be non-zero.
            // Chance of this happening is infinitesimal but we cover it anyway
            while ((_s0 | _s1 | _s2 | _s3) == 0);


            // Implements SplitMix64 algorithm: https://prng.di.unimi.it/splitmix64.c
            // From https://prng.di.unimi.it/:
            // "We suggest to use SplitMix64 to initialize the state of our generators starting from a 64-bit seed,
            //  as research has shown (https://dl.acm.org/doi/10.1145/1276927.1276928) that initialization must be
            //  performed with a generator radically different in nature from the one initialized to avoid correlation
            //  on similar seeds.
            ulong GetNextSplitMix64RandomValue()
            {
                ulong z = (x += 0x9e3779b97f4a7c15);
                z = (z ^ (z >> 30)) * 0xbf58476d1ce4e5b9;
                z = (z ^ (z >> 27)) * 0x94d049bb133111eb;
                return z ^ (z >> 31);
            }
        }





        /// <summary>
        /// Gets the next random 64 bits in this seeded sequence, in the range [<see cref="ulong.MinValue"/>, <see cref="ulong.MaxValue"/>]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ulong GetNextRandom64Bits()
        {
            ulong result = BitOperations.RotateLeft(_s1 * 5, 7) * 9;

            JumpOverOneValue();

            return result;
        }

        /// <summary>
        /// Skips over the next value in this seeded sequence. Is marginally faster than calling <see cref="GetNextRandom64Bits"/> and discarding the result.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void JumpOverOneValue()
        {
            ulong s0 = _s0, s1 = _s1, s2 = _s2, s3 = _s3;

            ulong t = s1 << 17;

            s2 ^= s0;
            s3 ^= s1;
            s1 ^= s2;
            s0 ^= s3;

            s2 ^= t;
            s3 = BitOperations.RotateLeft(s3, 45);

            _s0 = s0;
            _s1 = s1;
            _s2 = s2;
            _s3 = s3;
        }
    }
}
