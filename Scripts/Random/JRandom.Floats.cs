using System;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        /// <summary> Gets a random double-precision value in the range [0, 1]. </summary>
        public double FractionD()
        {
            // As described in http://prng.di.unimi.it/:
            // "A standard double (64-bit) floating-point number in IEEE floating point format has 52 bits of significand,
            //  plus an implicit bit at the left of the significand. Thus, the representation can actually store numbers with
            //  53 significant binary digits. Because of this fact, in C99 a 64-bit unsigned integer x should be converted to
            //  a 64-bit double using the expression
            //  (x >> 11) * 0x1.0p-53"
            return (GetSourceRandom64Bits() >> 11) * (1.0 / (1ul << 53));
        }

        /// <summary> Gets a random single-precision value in the range [0, 1]. </summary>
        public float Fraction()
        {
            // Same as above, but with 24 bits instead of 53.
            return (GetSourceRandom64Bits() >> 40) * (1.0f / (1u << 24));
        }


        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public double Range(double minInclusive, double maxInclusive)
        {
            if (minInclusive > maxInclusive)
                throw new ArgumentException($"{nameof(minInclusive)} cannot be greater than {nameof(maxInclusive)}");

            return FractionD() * (maxInclusive - minInclusive) + minInclusive;
        }

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public float Range(float minInclusive, float maxInclusive)
        {
            return (float)Range((double)minInclusive, (double)maxInclusive);
        }

        /// <summary> Gets a random value in the range [<paramref name="minInclusive"/>, <paramref name="maxInclusive"/>]. </summary>
        public decimal Range(decimal minInclusive, decimal maxInclusive)
        {
            return (decimal)Range((double)minInclusive, (double)maxInclusive);
        }



        /// <summary> Get a boolean value with a certain probability of being true. </summary>
        /// <param name="probabilityOfTrue"> 0 is always false, 1 is always true, 0.5 is 50/50 </param>
        public bool Chance(float probabilityOfTrue)
        {
            if (probabilityOfTrue <= 0) return false;
            if (probabilityOfTrue >= 1) return true;

            return GetSourceRandom64Bits() < (ulong)(probabilityOfTrue * ulong.MaxValue);
        }

        /// <summary> Get a boolean value with a certain probability of being true. </summary>
        /// <param name="probabilityOfTrue"> 0 is always false, 1 is always true, 0.5 is 50/50 </param>
        public bool Chance(double probabilityOfTrue)
        {
            if (probabilityOfTrue <= 0) return false;
            if (probabilityOfTrue >= 1) return true;

            return GetSourceRandom64Bits() < (ulong)(probabilityOfTrue * ulong.MaxValue);
        }


        /// <summary> 50% chance to be true, 50% chance to be false. </summary>
        public bool FiftyFifty()
            => (GetSourceRandom64Bits() & 1) == 0; // I benchmarked a few different ways of doing this method, this was the fastest


        /// <summary> 50% chance to be 1, 50% chance to be 0. </summary>
        public int OneOrZero()
            => (int)(GetSourceRandom64Bits() & 1);

        /// <summary> 50% chance to be 1, 50% chance to be -1. </summary>
        public int Sign()
            => (GetSourceRandom64Bits() & 2) - 1;


        /// <summary> Gets a random single-precision value in the range [-1, 1]. </summary>
        public float Scaler() // todo find better name for this function
            => Sign() * Fraction();

        /// <summary> Gets a random double-precision value in the range [-1, 1]. </summary>
        public double ScalerD()
            => Sign() * FractionD();
    }
}
