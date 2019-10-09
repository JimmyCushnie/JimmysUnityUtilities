using System;
using System.Globalization;
using UnityEngine;

namespace JimmysUnityUtilities
{
    /// <summary>
    /// Like UnityEngine.Color32 but without a transparency byte.
    /// </summary>
    [Serializable]
    public partial struct Color24
    {
        public byte r;
        public byte g;
        public byte b;

        public Color24(byte r, byte g, byte b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color24(int hex)
        {
            this.r = (byte)((hex & 0xFF0000) >> 16);
            this.g = (byte)((hex & 0xFF00) >> 8);
            this.b = (byte)(hex & 0xFF);
        }

        public Color32 WithOpacity(byte opacity = byte.MaxValue)
            => new Color32(r, g, b, opacity);


        public override string ToString() 
            => $"#{r:X2}{g:X2}{b:X2}";

        public static Color24 Parse(string hexCode)
        {
            if (hexCode.Length != 6)
                throw new Exception($"cannot parse {hexCode} as {nameof(Color24)}");

            return new Color24(Int32.Parse(hexCode, NumberStyles.HexNumber));
        }
    }
}
