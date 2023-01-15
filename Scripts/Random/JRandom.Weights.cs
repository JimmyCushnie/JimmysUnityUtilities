using System;
using System.Collections.Generic;
using System.Linq;

namespace JimmysUnityUtilities.Random
{
    // You might find it a bit odd that the public API is JRandom.WeightedSample(ProbabilityDistribution) instead of ProbabilityDistribution.Sample(JRandom).
    // I chose to do it like this for two reasons:
    //
    //     1. Consistency: every other random generation method is JRandom.Something(). Code that generates both random numbers and weighted random numbers
    //        shouldn't have to switch between different conventions.
    //     2. Readability: IMO, this way is more readable and clear. Your brain can parse it as "The random does a weighted sample of the probability
    //        distribution." This is subject-verb-object ordering which is quicker to understand than "The probability distribution gets sampled by the
    //        random" (object-verb-subject).
    //
    // I made this choice because for me, this style is easier to understand, and understandability is the most important thing when writing code. That's just
    // my opinion, though. You may have a different opinion; if so, I guess you have to suck it up 🤷💅

    public partial class JRandom
    {
        /// <summary>
        /// Samples a random item from a <see cref="ProbabilityDistribution{T}"/>, according to the relative weights of the items.
        /// </summary>
        public T WeightedSample<T>(ProbabilityDistribution<T> distribution)
            => distribution.Sample(this);
    }
}
