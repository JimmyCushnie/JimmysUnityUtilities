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
        /// <returns>Null if there's no file at the provided path or if the file failed to load.</returns>
        public static Texture2D LoadImageFromDisk(string path, LoadOptions options = new LoadOptions())
        {
            if (!File.Exists(path))
                return null;


            // See https://docs.unity3d.com/ScriptReference/ImageConversion.LoadImage.html for compression details
            // See https://docs.unity3d.com/ScriptReference/Texture2D-ctor.html for non-compression details
            TextureFormat format = options.CompressLoadedTextureInMemory ? TextureFormat.DXT1 : TextureFormat.RGBA32;

            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2, format, options.UseMipMaps);

            if (!texture.LoadImage(bytes, options.MarkLoadedTextureReadOnly))
                return null;
            
            return texture;
        }

        /// <summary>
        /// Options for <see cref="LoadImageFromDisk(string, LoadOptions)"/>
        /// </summary>
        public struct LoadOptions
        {
            public bool MarkLoadedTextureReadOnly;
            public bool UseMipMaps;
            public bool CompressLoadedTextureInMemory;

            /// <summary>
            /// Create a new instance of <see cref="LoadOptions"/>
            /// </summary>
            /// <param name="markLoadedTextureReadOnly">If true, the loaded texture will be read-only, and use much less memory.</param>
            /// <param name="useMipMaps">Whether the loaded texture should have mipmaps.</param>
            /// <param name="compressLoadedTextureInMemory">Whether to compress the loaded texture in memory. This cannot be true while useMipMaps is also true.</param>
            public LoadOptions(bool markLoadedTextureReadOnly = true, bool useMipMaps = true, bool compressLoadedTextureInMemory = false)
            {
                if (useMipMaps && compressLoadedTextureInMemory)
                    throw new System.Exception($"{nameof(useMipMaps)} and {nameof(compressLoadedTextureInMemory)} cannot both be true simultaneously"); // Sorry, graphics APIs are stupid

                this.MarkLoadedTextureReadOnly = markLoadedTextureReadOnly;
                this.UseMipMaps = useMipMaps;
                this.CompressLoadedTextureInMemory = compressLoadedTextureInMemory;
            }
        }
    }
}