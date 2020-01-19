namespace JimmysUnityUtilities
{
    public static class NumberExtensions
    {
        /// <summary>
        /// Used for effectively making a loop for a float, so that if it goes below zero it wraps around back to max, and if it goes above max it wraps around back to zero. </p>
        /// A common use is keeping degree floats between 0 and 360.
        /// </summary>
        public static float CapRange(this float value, float max)
        {
            // This is distinct from usuing modulus because it prevents negative values.
            while (value < 0) value += max;
            while (value > max) value -= max;
            return value;
        }

        /// <summary>
        /// Used for effectively making a loop for the number, so that if it goes below min it wraps around back to max, and if it goes above max it wraps around back to min.
        /// </summary>
        public static int CapRange(this int value, int max, int min = 0)
        {
            int diff = max - min;
            diff++;

            while (value < min) value += diff;
            while (value > max) value -= diff;
            return value;
        }

        public static int Clamp(this int value, int minInclusive, int maxInclusive)
        {
            if (value > maxInclusive) return maxInclusive;
            if (value < minInclusive) return minInclusive;
            return value;
        }

        public static bool IsBetween(this int value, int minInclusive, int maxInclusive)
        {
            if (value < minInclusive) return false;
            if (value > maxInclusive) return false;
            return true;
        }

        public static bool IsBetween(this float value, float minInclusive, float maxInclusive)
        {
            if (value < minInclusive) return false;
            if (value > maxInclusive) return false;
            return true;
        }

        public static bool IsEven(this int value)
            => value % 2 == 0;

        public static bool IsOdd(this int value)
            => !value.IsEven();

        /// <summary> This is usefully disctinct from Math.Pow because it uses integers. </summary>
        public static int ToThePowerOf(this int @base, int exponent)
        {
            if (exponent == 0) return 1;
            if (exponent == 1) return @base;

            int result = @base;
            for (int i = 1; i < exponent; i++)
                result *= @base;

            return result;
        }
    }
}