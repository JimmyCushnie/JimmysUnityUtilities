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
        /// <param name="filePath">The rooted path of the file. Extension optional.</param>
        public static void SaveImageToDisk(Texture2D texture, string filePath, ImageFormat format = ImageFormat.PNG)
        {
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
            
            filePath = Path.ChangeExtension(filePath, format.ToString().ToLower());
            string directoryPath = Path.GetDirectoryName(filePath);
            
            Directory.CreateDirectory(directoryPath); // ensure directory exists before we try to save to it
            File.WriteAllBytes(filePath, data);
        }

        /// <summary>
        /// Load a PNG or JPG from disk into a Texture2D.
        /// </summary>
        /// <param name="path">The full, rooted path of the file.</param>
        /// <returns>Null if there's no file at the provided path or if the file failed to load.</returns>
        public static Texture2D LoadImageFromDisk(string path)
            => LoadImageFromDisk(path, LoadOptions.Default);

        public static Texture2D LoadImageFromDisk(string path, LoadOptions options)
        {
            if (!File.Exists(path))
                return null;


            var bytes = File.ReadAllBytes(path);
            return LoadImageFromBytes(bytes, options);
        }

        /// <summary>
        /// Load a PNG or JPG from bytes into a Texture2D.
        /// </summary>
        /// <param name="bytes">All bytes contained in an image file.</param>
        /// <returns>Null if the bytes failed to load.</returns>
        public static Texture2D LoadImageFromBytes(byte[] bytes)
            => LoadImageFromBytes(bytes, LoadOptions.Default);

        public static Texture2D LoadImageFromBytes(byte[] bytes, LoadOptions options)
        {
            // See https://docs.unity3d.com/ScriptReference/ImageConversion.LoadImage.html for compression details
            // See https://docs.unity3d.com/ScriptReference/Texture2D-ctor.html for non-compression details
            var format = options.CompressLoadedTextureInMemory ? TextureFormat.DXT1 : TextureFormat.RGBA32;

            var texture = new Texture2D(4, 4, format, options.UseMipMaps); // Must be at least 4; 2x2 textures can't be created with mipmaps enabled. This, of course, isn't documented anywhere, and the error you get doesn't mention it at all. Fuck you unity

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
            /// <param name="compressLoadedTextureInMemory">Whether to compress the loaded texture in memory.</param>
            public LoadOptions(bool markLoadedTextureReadOnly = true, bool useMipMaps = true, bool compressLoadedTextureInMemory = true)
            {
                this.MarkLoadedTextureReadOnly = markLoadedTextureReadOnly;
                this.UseMipMaps = useMipMaps;
                this.CompressLoadedTextureInMemory = compressLoadedTextureInMemory;
            }

            public static LoadOptions Default => new LoadOptions(true, true, true);
        }
    }
}