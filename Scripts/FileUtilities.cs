using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JimmysUnityUtilities
{
    public static class FileUtilities
    {
        public const string FileTimestampFormat = "yyyy-MM-dd@HH-mm-ss";
        public static string CurrentTimestamp => DateTime.Now.ToLocalTime().ToString(FileTimestampFormat);

        public static bool TryParseDateTimeFromFileTimestamp(string timeStamp, out DateTime result)
            => DateTime.TryParseExact(timeStamp, FileTimestampFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);


        /// <summary> replaces any characters that cannot be in a file name </summary>
        /// <param name="replacement"> invalid characters will be replaced with this character </param>
        public static string ValidatedFileName(string name, char replacement = '_')
        {
            // https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
            // Todo support the rest of this nonsense

            if (name.IsNullOrEmpty())
                name = replacement.ToString();

            string validatedName = name.ReplaceAny(Path.GetInvalidFileNameChars(), replacement, 0);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var illegalName in IllegalWindowsFileNames)
                {
                    if (validatedName.Equals(illegalName, StringComparison.OrdinalIgnoreCase) ||
                        validatedName.StartsWith(illegalName + '.', StringComparison.OrdinalIgnoreCase))
                        return validatedName.Insert(illegalName.Length, "_");
                }
            }

            return validatedName;
        }
        static string[] IllegalWindowsFileNames = new string[]
        {
            "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
        };
        
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

        

        // Based on https://docs.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
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

            var directory = new DirectoryInfo(directoryPath);
            return directory.EnumerateFiles("*", SearchOption.AllDirectories).Sum(file => file.Length);
        }

        /// <summary>
        /// When the OS reports a directory's last write time, it usually doesn't include when items inside the directory were written to.
        /// This function iterates through a directory's files to find the last write time of any file within.
        /// </summary>
        public static DateTime GetDirectoryLastWriteTimeIncludingSubFiles(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Couldn't find directory {directoryPath}");

            var directory = new DirectoryInfo(directoryPath);
            var latestTime = directory.LastWriteTime;

            foreach (var file in directory.EnumerateFiles("*", SearchOption.AllDirectories))
            {
                if (file.LastWriteTime > latestTime)
                    latestTime = file.LastWriteTime;
            }

            return latestTime;
        }


        /// <summary>
        /// Returns strings like "13 B", "5.3 KB" ect
        /// </summary>
        public static string ByteCountToHumanReadableString(long byteCount, int decimalsToRoundTo = 1)
        {
            string[] byteSuffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
            const int @base = 1000; // 1024 for binary, 1000 for SI

            if (byteCount == 0)
                return "0" + byteSuffixes[0];

            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, @base)));
            double num = Math.Round(bytes / Math.Pow(@base, place), decimalsToRoundTo);

            return (Math.Sign(byteCount) * num).ToString() + " " + byteSuffixes[place];
        }

        // Todo: should also have a BitCountToHumanReadableString for download & upload speeds

        public static string PrettyFileSize(string filePath, int decimalsToRoundTo = 1)
            => ByteCountToHumanReadableString(new FileInfo(filePath).Length, decimalsToRoundTo);

        public static string PrettyDirectorySize(string directoryPath, int decimalsToRoundTo = 1)
            => ByteCountToHumanReadableString(GetDirectorySizeInBytes(directoryPath), decimalsToRoundTo);


        public static void OpenInFileExplorer(string path)
            => System.Diagnostics.Process.Start(path);




        // Somehow, .NET totally lacks this functionality, so we have to add it ourselves
        /// <summary>
        /// Converts a file path to a URI path.
        /// </summary>
        /// <param name="filePath">The full, rooted path of the file or directory</param>
        public static string FilePathToURI(string filePath)
        {
            StringBuilder uriBuilder = new StringBuilder(filePath);
            for (int i = 0; i < uriBuilder.Length; i++)
            {
                char c = uriBuilder[i];

                if (AsciiCharactersAllowedInURIWithoutEscaping.Contains(c)) // This will be the vast majority of characters, so checking this first is best for performance
                    continue;

                if (c > '\xFF') // Not an ascii character
                    continue;

                if (c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar)
                {
                    uriBuilder[i] = '/';
                    continue;
                }


                int charAsInt = (int)c;
                string escapedCharacter = charAsInt.ToString("X2");

                uriBuilder[i] = '%';
                uriBuilder.Insert(i + 1, escapedCharacter);

                i += escapedCharacter.Length - 1;
            }


            if (uriBuilder.Length >= 2 && uriBuilder[0] == '/' && uriBuilder[1] == '/') // UNC path
                uriBuilder.Insert(0, "file:");
            else
                uriBuilder.Insert(0, "file:///");

            return uriBuilder.ToString();
        }

        static readonly HashSet<char> AsciiCharactersAllowedInURIWithoutEscaping = GetAsciiCharactersAllowedInURI();
        static HashSet<char> GetAsciiCharactersAllowedInURI()
        {
            var set = new HashSet<char>();

            for (int i = 'a'; i <= 'z'; i++)
                set.Add((char)i);

            for (int i = 'A'; i <= 'Z'; i++)
                set.Add((char)i);

            for (int i = '0'; i <= '9'; i++)
                set.Add((char)i);

            set.Add('+');
            set.Add('/');
            set.Add(':');
            set.Add('.');
            set.Add('-');
            set.Add('_');
            set.Add('~');

            return set;
        }
    }
}