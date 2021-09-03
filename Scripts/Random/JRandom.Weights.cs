using System.Collections.Generic;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        public T WeightedRandom<T>(params (int weight, T value)[] options)
            => WeightedRandom(options as IReadOnlyList<(int weight, T value)>);

        public T WeightedRandom<T>(IReadOnlyList<(int weight, T value)> options)
        {
            // TODO: algorithm that isn't stupid
            // TODO: support floats/doubles for weight
            // TODO: support something better for parameters. Dictionaries?
            var indexedOptions = new List<T>();
            foreach (var option in options)
            {
                for (int i = 0; i < option.weight; i++)
                    indexedOptions.Add(option.value);
            }

            return RandomElementOf(indexedOptions);
        }
    }
}
