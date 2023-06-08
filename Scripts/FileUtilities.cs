using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using JimmysUnityUtilities.Random;

namespace JimmysUnityUtilities
{
    public static class FileUtilities
    {
        public const string FileTimestampFormat = "yyyy-MM-dd@HH-mm-ss";
        public static string CurrentTimestamp => DateTime.Now.ToLocalTime().ToString(FileTimestampFormat, CultureInfo.InvariantCulture);

        public static bool TryParseDateTimeFromFileTimestamp(string timeStamp, out DateTime result)
            => DateTime.TryParseExact(timeStamp, FileTimestampFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out result);


        static readonly IReadOnlyList<string> IllegalWindowsFileNames = new string[]
        {
            "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
        };
        public static bool IsLegalFileSystemPath(string path)
        {
            // https://docs.microsoft.com/en-us/windows/win32/fileio/naming-a-file
            // Todo support the rest of this nonsense

            if (path.Any(c => Path.GetInvalidPathChars().Contains(c)))
                return false;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                string fileName = Path.GetFileName(path);
                foreach (var illegalName in IllegalWindowsFileNames)
                {
                    if (fileName.Equals(illegalName, StringComparison.OrdinalIgnoreCase) ||
                        fileName.StartsWith(illegalName + '.', StringComparison.OrdinalIgnoreCase))
                        return false;
                }
            }

            return true;
        }

        /// <summary> replaces any characters that cannot be in a file name </summary>
        /// <param name="replacement"> invalid characters will be replaced with this character </param>
        public static string ValidatedFileName(string name, char replacement = '_')
        {
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

        /// <summary>
        /// You provide a parent directory and a desired name. We return that name, modified if necessary to make sure it is a valid path name and doesn't already exist.
        /// </summary>
        /// <param name="parentPath"> This path must be absolute </param>
        public static string ValidatedUniqueDirectoryName(string parentPath, string desiredName, string append = "-")
        {
            desiredName = ValidatedFileName(desiredName);

            while (Directory.Exists(Path.Combine(parentPath, desiredName)))
                desiredName += append;

            return Path.Combine(parentPath, desiredName);
        }

        /// <summary>
        /// Provide a path for a file you want to create. If that file already exists, the filename (before the extension) will be modified with a number after it to make it unique.
        /// </summary>
        public static string MakeProposedFilePathUnique(string desiredFilePath)
        {
            // todo use the improvements in this method for the directory one

            string newFilePath = desiredFilePath;
            int counter = 0;
            while (File.Exists(newFilePath))
            {
                counter++;
                newFilePath = AppendToFilenameWithoutChangingExtension(desiredFilePath, $" ({counter})");
            }

            return newFilePath;
        }

        /// <summary> Returns a full file path </summary>
        public static string ChangeFileNameWithoutChangingExtension(string originalFilePath, string newFileName)
        {
            string parentDirectory = Path.GetDirectoryName(originalFilePath);
            string extension = Path.GetExtension(originalFilePath);

            return Path.Combine(parentDirectory, newFileName + extension);
        }

        /// <summary> Returns a full file path </summary>
        public static string AppendToFilenameWithoutChangingExtension(string originalFilePath, string append)
        {
            string parentDirectory = Path.GetDirectoryName(originalFilePath);
            string fileName = Path.GetFileNameWithoutExtension(originalFilePath);
            string extension = Path.GetExtension(originalFilePath);

            return Path.Combine(parentDirectory, fileName + append + extension);
        }



        public static void CopyDirectory(string sourcePath, string destinationPath)
        {
            if (string.IsNullOrEmpty(sourcePath))
                throw new ArgumentException($"{nameof(sourcePath)} cannot be null or empty.", nameof(sourcePath));

            if (string.IsNullOrEmpty(destinationPath))
                throw new ArgumentException($"{nameof(destinationPath)} cannot be null or empty.", nameof(destinationPath));

            if (!FileUtilities.IsLegalFileSystemPath(sourcePath))
                throw new ArgumentException($"{nameof(sourcePath)} is illegal: {sourcePath}", nameof(sourcePath));

            if (!FileUtilities.IsLegalFileSystemPath(destinationPath))
                throw new ArgumentException($"{nameof(destinationPath)} is illegal: {destinationPath}", nameof(destinationPath));


            var sourceDirectory = new DirectoryInfo(sourcePath);

            if (!sourceDirectory.Exists)
                throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourcePath}");

            if (Directory.Exists(destinationPath))
                throw new Exception($"Destination path already exists: {destinationPath}");

            if (FileUtilities.IsSubpathOrSamePath(potentialChildPath: destinationPath, potentialParentPath: sourcePath))
                throw new Exception($"{nameof(destinationPath)} must not be a child of {nameof(sourcePath)}! ({destinationPath}, {sourcePath})");


            // Trim the end so there aren't errors with getting the relative path below
            sourcePath = Path.GetFullPath(sourcePath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Recursion seems the natural way to implement this function, but I've decided to go with an iterative approach for a few reasons:
            // - Better performance, especially when there are a lot of nested subdirectories
            // - No risk of stack overflow with especially deeply nested directories
            // - Easier to implement new features like parallelism, pause/resume copying, etc
            var directoriesToCopy = new Stack<DirectoryInfo>();
            directoriesToCopy.Push(sourceDirectory);

            while (directoriesToCopy.Count > 0)
            {
                var currentDirectory = directoriesToCopy.Pop();
                string relativePath = currentDirectory.FullName.Substring(startIndex: sourcePath.Length); // todo: use Path.GetRelativePath once we make it to .NET Standard 2.1
                string newDirectoryPath = Path.Combine(destinationPath, relativePath);

                try
                {
                    Directory.CreateDirectory(newDirectoryPath);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Unable to create directory at {newDirectoryPath}", ex);
                }

                // Copy all the files in this directory
                foreach (FileInfo file in currentDirectory.GetFiles())
                {
                    string newFilePath = Path.Combine(newDirectoryPath, file.Name);
                    try
                    {
                        file.CopyTo(newFilePath);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Unable to copy file at {file.FullName} to {newFilePath}", ex);
                    }
                }

                // Add all the subdirectories to the stack
                foreach (DirectoryInfo subdirectory in currentDirectory.GetDirectories())
                {
                    directoriesToCopy.Push(subdirectory);
                }
            }
        }

        public static bool IsSubpathOrSamePath(string potentialChildPath, string potentialParentPath)
        {
            potentialChildPath = Path.GetFullPath(potentialChildPath);
            potentialParentPath = Path.GetFullPath(potentialParentPath);

            if (String.Equals(potentialChildPath, potentialParentPath, StringComparison.OrdinalIgnoreCase))
                return true;

            if (potentialChildPath.StartsWith(potentialParentPath, StringComparison.OrdinalIgnoreCase))
            {
                if (potentialChildPath[potentialParentPath.Length] == Path.DirectorySeparatorChar ||
                    potentialChildPath[potentialParentPath.Length] == Path.AltDirectorySeparatorChar)
                    return true;
            }

            return false;
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
            var uriBuilder = new StringBuilder(filePath);
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


        /// <summary>
        /// Creates a new directory with a random name in the user's temp folder. Returns the full path of the new directory.
        /// </summary>
        public static string CreateTemporaryDirectory()
        {
            var tempPath = Path.GetTempPath();
            var directoryName = ValidatedUniqueDirectoryName(tempPath, JRandom.Shared.UppercaseAlphanumericString(10));
            var directoryPath = Path.Combine(tempPath, directoryName);

            Directory.CreateDirectory(directoryPath);
            return directoryPath;
        }


        public static bool FileContentsAreIdentical(string filePath1, string filePath2)
        {
            var fileInfo1 = new FileInfo(filePath1);
            var fileInfo2 = new FileInfo(filePath2);

            if (fileInfo1.Length != fileInfo2.Length)
                return false;


            using (FileStream fileStream1 = fileInfo1.OpenRead())
            using (FileStream fileStream2 = fileInfo2.OpenRead())
            {
                const int bufferSize = 4096;
                var buffer1 = new byte[bufferSize];
                var buffer2 = new byte[bufferSize];

                int bytesRead1;
                int bytesRead2;
                while (
                    (bytesRead1 = fileStream1.Read(buffer1, 0, bufferSize)) > 0 &&
                    (bytesRead2 = fileStream2.Read(buffer2, 0, bufferSize)) > 0)
                {
                    if (bytesRead1 != bytesRead2) // This should never happen, but, hey, better check it just in case
                        return false;

                    for (int i = 0; i < bytesRead1; i++)
                    {
                        if (buffer1[i] != buffer2[i])
                            return false;
                    }
                }
            }

            // Reached end of both files without finding any differences
            return true;
        }

        public static bool DirectoryExistsAndIsNotEmpty(string directoryPath)
        {
            return Directory.Exists(directoryPath) && Directory.EnumerateFileSystemEntries(directoryPath).Any();
        }
    }
}