using System;

namespace JimmysUnityUtilities.Random
{
    /// <summary>
    /// Seeded random value generation, with more built-in methods than <see cref="System.Random"/>.
    /// </summary>
    public partial class JRandom
    {
        /// <summary>
        /// Creates a new <see cref="JRandom"/> with a 64-bit seed.
        /// </summary>
        public JRandom(ulong seed)
        {
            _Generator = new VeryFastRandomValueGenerator(seed);
        }

        /// <summary>
        /// Creates a new <see cref="JRandom"/> with a pseudorandom seed.
        /// </summary>
        public JRandom() : this(Shared.UInt64())
        {
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
