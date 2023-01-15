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
        /// It's safe to access this from multiple different threads concurrently; each thread actually gets a different instance.
        /// </summary>
        public static JRandom Shared 
        {
            get
            {
                if (_Shared == null)
                {
                    lock (SharedInstanceSeederLock)
                    {
                        _Shared = new JRandom(SharedInstanceSeeder.GetNextRandom64Bits());
                    }
                }

                return _Shared;
            }
        }

        [ThreadStatic]
        private static JRandom _Shared;

        static readonly VeryFastRandomValueGenerator SharedInstanceSeeder = new VeryFastRandomValueGenerator((ulong)DateTime.Now.Ticks);
        static readonly object SharedInstanceSeederLock = new object();
    }
}
