using System;

namespace JimmysUnityUtilities.Random
{
    /// <summary>
    /// Seeded random generation, with more built-in methods than <see cref="System.Random"/>.
    /// </summary>
    public partial class JRandom
    {
        /// <summary>
        /// Creates a new <see cref="JRandom"/> with a pseudorandom seed.
        /// </summary>
        public JRandom() : this(GetPseudorandomSeed())
        {
        }
        static ulong GetPseudorandomSeed()
            => (ulong)DateTime.Now.Ticks;

        /// <summary>
        /// Creates a new seeded <see cref="JRandom"/>, using the hashcode of the provided object.
        /// </summary>
        public JRandom(object objectSeed) : this(objectSeed.GetHashCode())
        {
        }

        /// <summary>
        /// Creates a new <see cref="JRandom"/> with a 32-bit seed.
        /// </summary>
        public JRandom(int seed) : this((ulong)seed)
        {
        }

        /// <summary>
        /// Creates a new <see cref="JRandom"/> with a 64-bit seed.
        /// </summary>
        public JRandom(ulong seed)
        {
            _Generator = new VeryFastRandomValueGenerator(seed);
        }


        private VeryFastRandomValueGenerator _Generator;
        private ulong GetSourceRandom64Bits()
        {
            return _Generator.GetNextRandom64Bits();
        }



        /// <summary>
        /// Shared instance of <see cref="JRandom"/> so that you can sporadically get a pseudorandom value without having to instantiate a new instance.
        /// </summary>
        public static JRandom Shared { get; } = new JRandom(); // I don't think this is thread safe, but I'm not sure lol. Todo, verify that it's not, then make it thread-safe
    }
}
