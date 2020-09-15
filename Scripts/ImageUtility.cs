using UnityEngine;
using System.IO;

namespace JimmysUnityUtilities
{
    public enum ImageFormat
    {
        PNG = 0,
        JPG,
    }

    public static class ImageUtility
    {
        /// <summary>
        /// Save a Texture2D to disk as PNG or JPG.
        /// </summary>
        /// <param name="path">The rooted path of the file. Extension optional.</param>
        public static void SaveImageToDisk(Texture2D texture, string path, ImageFormat format = ImageFormat.PNG)
        {
            path = Path.ChangeExtension(path, format.ToString());

            byte[] data;
            switch (format)
            {
                default:
                case ImageFormat.PNG:
                    data = texture.EncodeToPNG();
                    break;
                case ImageFormat.JPG:
                    data = texture.EncodeToJPG();
                    break;
            }

            File.WriteAllBytes(path, data);
        }

        /// <summary>
        /// Load a PNG or JPG from disk into a Texture2D.
        /// </summary>
        /// <param name="path">The full, rooted path of the file.</param>
        /// <param name="compressLoadedTextureInMemory">Whether to compress the loaded texture in memory.</param>
        /// <param name="markLoadedTextureReadOnly">If true, the loaded texture will be read-only, and use much less memory.</param>
        /// <param name="useMipMaps">Whether the loaded texture should have mipmaps.</param>
        /// <returns>Null if there's no file at the provided path or if the file failed to load.</returns>
        public static Texture2D LoadImageFromDisk(string path, bool compressLoadedTextureInMemory = true, bool markLoadedTextureReadOnly = true)
        {
            if (!File.Exists(path))
                return null;


            // See https://docs.unity3d.com/ScriptReference/ImageConversion.LoadImage.html for compression details
            // See https://docs.unity3d.com/ScriptReference/Texture2D-ctor.html for non-compression details
            TextureFormat format = compressLoadedTextureInMemory ? TextureFormat.DXT1 : TextureFormat.RGBA32;

            // idk why but making the texture use mipmaps causes a fuckton of dx11 errors
            const bool useMipMaps = false;

            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2, format, useMipMaps);

            if (!texture.LoadImage(bytes, markLoadedTextureReadOnly))
                return null;
            
            return texture;
        }
    }
}