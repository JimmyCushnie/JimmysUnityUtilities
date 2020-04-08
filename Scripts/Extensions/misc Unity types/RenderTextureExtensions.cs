using UnityEngine;

namespace JimmysUnityUtilities
{
    public static class RenderTextureExtensions
    {
        public static Texture2D ToTexture2D(this RenderTexture source)
        {
            var previouslyActiveRenderTexture = RenderTexture.active;
            RenderTexture.active = source;

            var texture = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
            texture.Apply();

            RenderTexture.active = previouslyActiveRenderTexture;
            return texture;
        }
    }
}