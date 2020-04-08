using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace JimmysUnityUtilities
{
    public static class FileUtilities
    {
        public static string CurrentTimestamp => DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd@HH-mm-ss");

        /// <summary> replaces any characters that cannot be in a file name </summary>
        /// <param name="replacement"> invalid characters will be replaced with this character </param>
        public static string ValidatedFileName(string name, string replacement = "_")
        {
            Regex searcher = new Regex("[" + new string(Path.GetInvalidFileNameChars()) + "]"); // the characters between [ and ] are the characters to search for - all the invalid file name characters
            return searcher.Replace(name, replacement);
        }
        
        /// <summary>
        /// You provide a parent directory and a desired name. We return that name, modified if necessary to make sure it is a valid path name and doesn't already exist.
        /// </summary>
        /// <param name="parentPath"> This path must be absolute </param>
        public static string ValidatedUniqueDirectoryName(string parentPath, string desiredName, string append = "-")
        {
            desiredName = ValidatedFileName(desiredName);

            while (Directory.Exists(Path.Combine(parentPath, desiredName)))
                desiredName = desiredName + append;

            return Path.Combine(parentPath, desiredName);
        }

        

        // based on https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
        public static void CopyDirectory(string sourcePath, string destinationPath, bool copySubDirectories = true)
        {
            DirectoryInfo souce = new DirectoryInfo(sourcePath);

            if (!souce.Exists)
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourcePath}");

            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);
            
            foreach (FileInfo file in souce.GetFiles())
            {
                string newPath = Path.Combine(destinationPath, file.Name);
                file.CopyTo(newPath, false);
            }
            
            if (copySubDirectories)
            {
                foreach (DirectoryInfo subdirectory in souce.GetDirectories())
                {
                    string newPath = Path.Combine(destinationPath, subdirectory.Name);
                    CopyDirectory(subdirectory.FullName, newPath, copySubDirectories);
                }
            }
        }

        public static long GetDirectorySizeInBytes(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Couldn't find directory {directoryPath}");

            var info = new DirectoryInfo(directoryPath);
            return info.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
        }

        /// <summary>
        /// Returns strings like 13B, 5.3KB ect
        /// </summary>
        public static string ByteCountToHumanReadableString(long byteCount, int decimalsToRoundTo = 1)
        {
            string[] byteSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            const int @base = 1024; // 1024 for binary, 1000 for SI

            if (byteCount == 0)
                return "0" + byteSuffixes[0];

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, @base)));
            double num = Math.Round(bytes / Math.Pow(@base, place), decimalsToRoundTo);

            return (Math.Sign(byteCount) * num).ToString() + byteSuffixes[place];
        }

        // todo: should also have a BitCountToHumanReadableString for download & upload speeds

        public static string PrettyFileSize(string filePath, int decimalsToRoundTo = 1)
            => ByteCountToHumanReadableString(new FileInfo(filePath).Length, decimalsToRoundTo);

        public static string PrettyDirectorySize(string directoryPath, int decimalsToRoundTo = 1)
            => ByteCountToHumanReadableString(GetDirectorySizeInBytes(directoryPath), decimalsToRoundTo);


        public static void OpenInFileExplorer(string path)
            => System.Diagnostics.Process.Start(path);
    }
}