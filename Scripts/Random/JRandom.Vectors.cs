using UnityEngine;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        /// <summary> Gets a random vector where each component is randomized between that component's value in <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector2 Range(Vector2 minInclusive, Vector2 maxInclusive)
            => new Vector2(Range(minInclusive.x, maxInclusive.x), Range(minInclusive.y, maxInclusive.y));

        /// <summary> Gets a random vector where each component is randomized between that component's value in <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector2Int Range(Vector2Int minInclusive, Vector2Int maxInclusive)
            => new Vector2Int(Range(minInclusive.x, maxInclusive.x), Range(minInclusive.y, maxInclusive.y));

        /// <summary> Gets a random vector where each component is randomized between that component's value in <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector3 Range(Vector3 minInclusive, Vector3 maxInclusive)
            => new Vector3(Range(minInclusive.x, maxInclusive.x), Range(minInclusive.y, maxInclusive.y), Range(minInclusive.z, maxInclusive.z));

        /// <summary> Gets a random vector where each component is randomized between that component's value in <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector3Int Range(Vector3Int minInclusive, Vector3Int maxInclusive)
            => new Vector3Int(Range(minInclusive.x, maxInclusive.x), Range(minInclusive.y, maxInclusive.y), Range(minInclusive.z, maxInclusive.z));

        /// <summary> Gets a random vector where each component is randomized between that component's value in <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector4 Range(Vector4 minInclusive, Vector4 maxInclusive)
            => new Vector4(Range(minInclusive.x, maxInclusive.x), Range(minInclusive.y, maxInclusive.y), Range(minInclusive.z, maxInclusive.z), Range(minInclusive.w, maxInclusive.w));



        /// <summary> Gets a vector where each component has a random value between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector2 Vector2(float minInclusive, float maxInclusive)
            => new Vector2(Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive));

        /// <summary> Gets a vector where each component has a random value between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector2Int Vector2Int(int minInclusive, int maxInclusive)
            => new Vector2Int(Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive));

        /// <summary> Gets a vector where each component has a random value between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector3 Vector3(float minInclusive, float maxInclusive)
            => new Vector3(Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive));

        /// <summary> Gets a vector where each component has a random value between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector3Int Vector3Int(int minInclusive, int maxInclusive)
            => new Vector3Int(Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive));

        /// <summary> Gets a vector where each component has a random value between <paramref name="minInclusive"/> and <paramref name="maxInclusive"/>. </summary>
        public Vector4 Vector4(float minInclusive, float maxInclusive)
            => new Vector4(Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive), Range(minInclusive, maxInclusive));






        // Todo, more interesting random vector functions
    }
}
