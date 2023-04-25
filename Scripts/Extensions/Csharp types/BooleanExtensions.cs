namespace JimmysUnityUtilities
{
    public static class BooleanExtensions
    {
        /// <summary> If true, returns 1; if false, returns -1 </summary>
        public static int Sign(this bool value)
            => value ? 1 : -1;

        /// <summary> If true, returns 1; if false, returns 0 </summary>
        public static int Bit(this bool value)
            => value ? 1 : 0;
    }
}