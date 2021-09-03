using UnityEngine;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        /// <summary> Gets a random rotation in 3D space as a Quaternion. </summary>
        public Quaternion Rotation3D()
        {
            return new Quaternion(Fraction(), Fraction(), Fraction(), Fraction()).normalized;
        }

        // Todo: fix the Rotation3D method so it isn't biased
        // https://stackoverflow.com/a/56794499
        // https://en.wikipedia.org/wiki/Hopf_fibration

        // Todo, more interesting random quaternion functions
    }
}
