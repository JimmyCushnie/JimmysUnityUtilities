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
        /// <returns>Null if there's no file at the provided path.</returns>
        public static Texture2D LoadImageFromDisk(string path)
        {
            if (!File.Exists(path))
                return null;

            var bytes = File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);

            return texture;
        }
    }
}