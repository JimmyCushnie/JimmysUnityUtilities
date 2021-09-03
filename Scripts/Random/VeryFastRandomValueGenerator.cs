using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine;

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
        /// Creates a <see cref="VeryFastRandomValueGenerator"/> with a convenient integer seed.
        /// </summary>
        public VeryFastRandomValueGenerator(int seed)
        {
            var systemRandom = new System.Random(seed);

            do
            {
                _s0 = GetSystemRandomUlong();
                _s1 = GetSystemRandomUlong();
                _s2 = GetSystemRandomUlong();
                _s3 = GetSystemRandomUlong();
            }
            while ((_s0 | _s1 | _s2 | _s3) == 0); // Chance of this is infinitesimal but we cover it anyway


            ulong GetSystemRandomUlong()
            {
                ulong left = (ulong)systemRandom.Next();
                ulong right = (ulong)systemRandom.Next();

                return (left << 32) | right;
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
