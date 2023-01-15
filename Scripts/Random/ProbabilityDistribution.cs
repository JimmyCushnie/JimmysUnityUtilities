
// For weighted randoms we use the Alias Method, which is an ingenious method for non-uniform random variate generation.
// We use the Vose algorithm to account for potential floating point shenanigans.
//
// Resources:
// - https://en.wikipedia.org/wiki/Alias_method
// - https://lips.cs.princeton.edu/the-alias-method-efficient-sampling-with-many-discrete-outcomes/
// - https://www.keithschwarz.com/darts-dice-coins/ (this one has a great visual explanation)
//
// Given an O(n) initialization, we can sample the probability distribution very fast (O(1)).
// It will be hard to understand this code if you don't understand the algorithm; I'm not going to try explaining it here.


using System;
using System.Collections.Generic;
using System.Linq;

namespace JimmysUnityUtilities.Random
{
    /// <summary>
    /// A simple struct containing an item and a relative weight. Used to construct <see cref="ProbabilityDistribution{T}"/>s.
    /// </summary>
    public readonly struct WeightedItem<T>
    {
        public readonly float Weight;
        public readonly T Item;

        public WeightedItem(float weight, T item)
        {
            if (weight < 0)
                throw new ArgumentException($"{nameof(weight)} must not be below zero");

            Weight = weight;
            Item = item;
        }
    }


    /// <summary>
    /// Represents a discrete probability distribution, with a bunch of items that each have a certain weighted probability of being chosen.
    /// Immutable once created.
    /// Item weights are relative to each other.
    /// Create one using <see cref="ProbabilityDistribution{T}.Create(IReadOnlyList{WeightedItem{T}})"/> or one of the overloads.
    /// Sample from it using <see cref="JRandom.WeightedSample{T}(ProbabilityDistribution{T})"/>.
    /// </summary>
    public class ProbabilityDistribution<T>
    {
        public readonly int ItemsCount;

        private readonly T[] ItemsTable;
        private readonly T[] AliasTable;
        private readonly float[] ProbabilityTable;

        internal T Sample(JRandom random)
        {
            int index = random.Range(0, ItemsCount - 1);

            if (random.Chance(ProbabilityTable[index]))
                return ItemsTable[index];

            return AliasTable[index];
        }


        private ProbabilityDistribution(int itemsCount, T[] itemsTable, T[] aliasTable, float[] probabilityTable)
        {
            ItemsCount = itemsCount;
            ItemsTable = itemsTable;
            AliasTable = aliasTable;
            ProbabilityTable = probabilityTable;
        }


        /// <summary> Creates a <see cref="ProbabilityDistribution{T}"/> of items with relative weights. </summary>
        public static ProbabilityDistribution<T> Create(params (float weight, T item)[] itemsAndWeights)
            => Create(itemsAndWeights.Select(x => new WeightedItem<T>(x.weight, x.item)).ToList());

        /// <summary> Creates a <see cref="ProbabilityDistribution{T}"/> of items with relative weights. </summary>
        public static ProbabilityDistribution<T> Create(params WeightedItem<T>[] itemsAndWeights)
            => Create(itemsAndWeights as IReadOnlyList<WeightedItem<T>>);

        /// <summary> Creates a <see cref="ProbabilityDistribution{T}"/> of items with relative weights. </summary>
        public static ProbabilityDistribution<T> Create(IReadOnlyList<WeightedItem<T>> itemsAndWeights)
        {
            if (itemsAndWeights == null)
                throw new ArgumentNullException($"{nameof(itemsAndWeights)} cannot be null");

            if (itemsAndWeights.Count == 0)
                throw new ArgumentException($"{nameof(itemsAndWeights)} must contain at least one item");


            int itemsCount = itemsAndWeights.Count;


            // Temporary tables used to fill the data tables
            var heights = new float[itemsCount];
            var overfullIndexes = new Stack<int>();
            var underfullIndexes = new Stack<int>();

            float totalWeight = itemsAndWeights.Sum(i => i.Weight);
            if (totalWeight <= 0f)
                throw new ArgumentException($"At least one entry in {nameof(itemsAndWeights)} must be above zero");

            for (int i = 0; i < itemsAndWeights.Count; i++)
            {
                // Scale the weights such that an item with a probability of 1/itemsCount would have height 1
                float height = (itemsAndWeights[i].Weight / totalWeight) * itemsCount;
                heights[i] = height;

                // Sort the indexes into underfull/overfull stacks
                if (height < 1.0f)
                    underfullIndexes.Push(i);
                else
                    overfullIndexes.Push(i);
            }


            // Data tables that are used when sampling the probability distribution
            var itemsTable = itemsAndWeights.Select(x => x.Item).ToArray();
            var aliasTable = new T[itemsCount];
            var probabilityTable = new float[itemsCount];

            while (underfullIndexes.Count > 0 && overfullIndexes.Count > 0)
            {
                int underfullIndex = underfullIndexes.Pop();
                int overfullIndex = overfullIndexes.Pop();

                float underfullHeight = heights[underfullIndex];
                float overfullHeight = heights[overfullIndex];
                               

                probabilityTable[underfullIndex] = underfullHeight;
                aliasTable[underfullIndex] = itemsTable[overfullIndex];


                float newOverfullHeight = overfullHeight + underfullHeight - 1; // Equivalent to `overfullHeight - (1 - underfullHeight)` but more numerically stable
                heights[overfullIndex] = newOverfullHeight;

                if (newOverfullHeight < 1.0f)
                    underfullIndexes.Push(overfullIndex);
                else
                    overfullIndexes.Push(overfullIndex);
            }


            // OK, all the remaining indexes are in one stack now, which means that these remaining indexes should have a probability of 1/Count -- i.e. their
            // ProbabilityTable entry should be 1.0.
            // Due to floating point instability, we can't be sure *which* stack they're in. With perfect precision, they'd all be in overfullIndexes.

            foreach (var index in overfullIndexes)
                probabilityTable[index] = 1.0f;

            foreach (var index in underfullIndexes)
                probabilityTable[index] = 1.0f;


            return new ProbabilityDistribution<T>(itemsCount, itemsTable, aliasTable, probabilityTable);
        }
    }
}
