using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class TextureUtilities
    {
        /// <summary>
        /// You may specify a rectangle that is partially or entirely outside the bounds of the texture; only pixels within bounds will be drawn.
        /// Note that you still need to call <see cref="Texture2D.Apply"/> after using this method.
        /// </summary>
        public static void DrawRectangleOn(this Texture2D texture, Vector2Int rectStart, Vector2Int rectEnd, Color color)
        {
            for (int x = rectStart.x; x <= rectEnd.x; x++)
            {
                if (!x.IsBetween(0, texture.width - 1))
                    continue;

                for (int y = rectStart.y; y <= rectEnd.y; y++)
                {
                    if (!y.IsBetween(0, texture.height - 1))
                        continue;

                    texture.SetPixel(x, y, color);
                }
            }
        }
    }
}