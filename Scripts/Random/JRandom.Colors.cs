using UnityEngine;

namespace JimmysUnityUtilities.Random
{
    public partial class JRandom
    {
        /// <summary> Gets a random <see cref="Color24"/> with random values for the red, green, and blue components. </summary>
        public Color24 Color24()
            => new Color24(Byte(), Byte(), Byte());


        /// <summary> Gets a random <see cref="Color32"/> with random values for the red, green, and blue components and an alpha of <paramref name="alpha"/>. </summary>
        public Color32 Color32(byte alpha = byte.MaxValue)
            => new Color32(Byte(), Byte(), Byte(), alpha);


        /// <summary> Gets a random <see cref="Color"/> with random values for the red, green, and blue components and an alpha of <paramref name="alpha"/>. </summary>
        public Color Color(float alpha = 1)
            => new Color(Fraction(), Fraction(), Fraction(), alpha);


        /// <summary> Gets a random <see cref="Color32"/> with random values for the red, green, blue, and alpha components. </summary>
        public Color32 Color32WithRandomAlpha()
            => Color32(Byte());


        /// <summary> Gets a random <see cref="Color"/> with random values for the red, green, blue, and alpha components. </summary>
        public Color ColorWithRandomAlpha()
            => Color(Fraction());
    }
}
