using System;
using System.Collections.Generic;
using UnityEngine;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Seeded random generation, with more built-in methods than <see cref="System.Random"/>
    /// </summary>
    public class JRandom
    {
        private System.Random _Random;


        /// <summary>
        /// Creates a new <see cref="JRandom"/> with a pseudorandom seed
        /// </summary>
        public JRandom(): this(GetPseudorandomSeed())
        {
        }
        static int GetPseudorandomSeed()
            => unchecked((int)(DateTime.Now.Ticks & long.MaxValue));

        /// <summary>
        /// Creates a new seeded <see cref="JRandom"/>, using the hashcode of the provided object.
        /// </summary>
        public JRandom(object objectSeed) : this(objectSeed.GetHashCode())
        {
        }

        /// <summary>
        /// Creates a new seeded <see cref="JRandom"/>
        /// </summary>
        public JRandom(int seed)
        {
            _Random = new System.Random(seed);
        }



        /// <summary>
        /// Random float between 0 and 1.
        /// </summary>
        public float Fraction()
            => (float)_Random.NextDouble();

        /// <summary>
        /// Random double between 0 and 1.
        /// </summary>
        public double FractionD()
            => _Random.NextDouble();

        /// <summary>
        /// Get a boolean value with a certain probability of being true.
        /// </summary>
        public bool Chance(float probabilityOfTrue)
            => Chance((double)probabilityOfTrue);

        /// <summary>
        /// Get a boolean value with a certain probability of being true. Default probability is 50/50.
        /// </summary>
        public bool Chance(double probabilityOfTrue = 0.5)
            => FractionD() < probabilityOfTrue;

        /// <summary>
        /// 50% chance to be 1, 50% chance to be -1
        /// </summary>
        public int Sign()
            => Chance() ? 1 : -1;

        /// <summary>
        /// Random float between -1 and 1.
        /// </summary>
        public float Scaler() // todo find better name for this function
            => Sign() * Fraction();


        public int Range(int minInclusive, int maxInclusive)
        {
            if (minInclusive > maxInclusive)
                throw new ArgumentException($"{nameof(minInclusive)} cannot be greater than {nameof(maxInclusive)}");

            return _Random.Next(minInclusive, maxInclusive + 1); // System.Random's max is exclusive
        }

        public float Range(float minInclusive, float maxInclusive)
        {
            if (minInclusive > maxInclusive)
                throw new ArgumentException($"{nameof(minInclusive)} cannot be greater than {nameof(maxInclusive)}");

            return Fraction() * (maxInclusive - minInclusive) + minInclusive;
        }



        public T RandomElementOf<T>(IReadOnlyList<T> list)
            => list[this.Range(0, list.Count - 1)];


        public T WeightedRandom<T>(params (int weight, T value)[] options)
            => WeightedRandom(options as IReadOnlyList<(int weight, T value)>);

        public T WeightedRandom<T>(IReadOnlyList<(int weight, T value)> options)
        {
            // TODO: algorithm that isn't stupid
            // TODO: support floats/doubles for weight
            // TODO: support dictionaries of <T, int>
            var indexedOptions = new List<T>();
            foreach (var option in options)
            {
                for (int i = 0; i < option.weight; i++)
                    indexedOptions.Add(option.value);
            }

            return RandomElementOf(indexedOptions);
        }


        /// <summary> In degrees. </summary>
        public float Angle()
            => Range(0f, 360f);

        public Vector2 PointOnUnitCircle() 
            => Vector2.up.Rotate(Angle());

        public Vector2 PointWithinUnitCircle()
            => PointOnUnitCircle() * Fraction();

        public Vector2 PointOnCircle(float radius)
            => PointOnUnitCircle() * radius;

        public Vector2 PointWithinCircle(float radius)
            => PointWithinUnitCircle() * radius;




        public byte Byte()
            => (byte)Range(byte.MinValue, byte.MaxValue);

        public sbyte SByte()
            => (sbyte)Range(sbyte.MinValue, sbyte.MaxValue);


        // TODO: random ranges for all the c# number types (uint, short, long, ect)
        // TODO: random vectors, quaternions, colors
    }
}