using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace JimmysUnityUtilities
{
    public static class GraphicExtensions
    {
        public static void SetAlpha(this Graphic graphic, float alpha) 
            => graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }
}