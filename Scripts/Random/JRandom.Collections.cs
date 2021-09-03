using System.Collections.Generic;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        public T RandomElementOf<T>(IReadOnlyList<T> list)
            => list[this.Range(0, list.Count - 1)];
    }
}
