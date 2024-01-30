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

        /// <summary> Takes an arbitrary string, typically one entered by a user, and converts it into a string that is valid to use as a file name. </summary>
        /// <param name="fixerChar"> This char replaces invalid chars in the name, and is added as necessary to keep the filename valid. </param>
        public static string ValidatedFileName(string name, char fixerChar = '_')
        {
            if (Path.GetInvalidFileNameChars().Contains(fixerChar)
                || fixerChar == ' ' || fixerChar == '.'
                || fixerChar == Path.DirectorySeparatorChar || fixerChar == Path.AltDirectorySeparatorChar)
                throw new ArgumentException("Fixer char is invalid", nameof(fixerChar));


            if (name.IsNullOrEmpty())
                name = fixerChar.ToString();

            string validatedName = name.ReplaceAny(Path.GetInvalidFileNameChars(), fixerChar, startIndex: 0);


            // Okay, so the fixes below are only strictly necessary on Windows, however for now I'm opting to apply them on all OSes.
            // The reason is that this method is typically used to get filenames for user-generated content, i.e. save files.
            // User-generated content is frequently shared between users, and we don't want there to be any friction in this sharing.
            // I.e. if a Linux user creates something, they should be able to just send it to their Windows friends, and the Windows
            // friends shouldn't run into awkward confusing file system errors.

            //if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                foreach (var illegalName in IllegalWindowsFileNames)
                {
                    if (validatedName.Equals(illegalName, StringComparison.OrdinalIgnoreCase) ||
                        validatedName.StartsWith(illegalName + '.', StringComparison.OrdinalIgnoreCase))
                        validatedName = validatedName.Insert(illegalName.Length, fixerChar.ToString());
                }

                if (validatedName.EndsWith(".") || validatedName.EndsWith(" "))
                    validatedName += fixerChar;
            }

            return validatedName;
        }

        /// <summary>
        /// Provide a path for a directory you want to create. If that directory already exists (or if a file exists at the same path), the directory name will be modified with a number after it to make it unique.
        /// </summary>
        public static string MakeProposedDirectoryPathUnique(string desiredDirectoryPath)
        {
            string newPath = desiredDirectoryPath;
            int counter = 0;
            while (File.Exists(newPath) || Directory.Exists(newPath))
            {
                counter++;
                newPath = desiredDirectoryPath + $" ({counter})";
            }

            return newPath;
        }

        /// <summary>
        /// Provide a path for a file you want to create. If that file already exists (or if a directory exists at the same path), the filename (before the extension) will be modified with a number after it to make it unique.
        /// </summary>
        public static string MakeProposedFilePathUnique(string desiredFilePath)
        {
            string newFilePath = desiredFilePath;
            int counter = 0;
            while (File.Exists(newFilePath) || Directory.Exists(newFilePath))
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

            if (FileUtilities.IsSubpathOrSamePath(potentialChildPath: destinationPath, potentialParentPath: sourcePath)) // Note that this could still be a problem if we're on a case-insensitive file system...
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
                string relativePath = currentDirectory.FullName.Substring(startIndex: sourcePath.Length) // todo: use Path.GetRelativePath once we make it to .NET Standard 2.1
                    .TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar); // Path.Combine breaks if an element begins with a DirectorySeparatorChar (it gets treated as a rooted path)
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


        /// <summary>
        /// Returns true if <paramref name="potentialChildPath"/> is a subpath of <paramref name="potentialParentPath"/> -- i.e. <paramref name="potentialChildPath"/> is contained within <paramref name="potentialParentPath"/>
        /// VERY IMPORTANT: even if you are on a case-insensitive file system, you MUST use the same casing for both paths, or you could have a false negative.
        /// </summary>
        /// <param name="potentialChildPath"> Path to a file or folder </param>
        /// <param name="potentialParentPath"> Path to a file or folder </param>
        public static bool IsSubpathOf(string potentialChildPath, string potentialParentPath)
            => TestIfSubpathInternal(potentialChildPath, potentialParentPath, alsoReturnTrueIfSamePath: false);

        /// <summary>
        /// Returns true if <paramref name="potentialChildPath"/> is a subpath of <paramref name="potentialParentPath"/>, or if they represent the same file/folder.
        /// VERY IMPORTANT: even if you are on a case-insensitive file system, you MUST use the same casing for both paths, or you could have a false negative.
        /// </summary>
        /// <param name="potentialChildPath"> Path to a file or folder </param>
        /// <param name="potentialParentPath"> Path to a file or folder </param>
        public static bool IsSubpathOrSamePath(string potentialChildPath, string potentialParentPath)
            => TestIfSubpathInternal(potentialChildPath, potentialParentPath, alsoReturnTrueIfSamePath: true);

        private static bool TestIfSubpathInternal(string potentialChildPath, string potentialParentPath, bool alsoReturnTrueIfSamePath)
        {
            // I really don't feel great about how the string comparisons are always case-sensitive. This can lead to false negatives, i.e. on a case-insensitive file system
            // where you're comparing `documents/taxes` and `Documents/Taxes/2019`.
            // However, I can't make the string comparison always case-insensitive, because then we'll get false positives on case-sensitive file systems!
            // Ideally, we'd check whether the file system referenced by each path is case-sensitive, and adjust the string comparison accordingly. However, as far as I can
            // tell this is impossible in C#.
            // We could use an heuristic like `caseSensitive = OS != Windows`, but this will NOT work in every situation and I prefer consistently non-ideal behavior to 
            // behavior that breaks sometimes in a sneaky hard-to-anticipate way.
            // I think being always-case-sensitive is the best compromise for now, but I'd really like to improve this in the future... maybe .NET will implement this function
            // in the standard library and I'll be able to just delete all this lol

            potentialChildPath = Path.GetFullPath(potentialChildPath);
            potentialParentPath = Path.GetFullPath(potentialParentPath);

            if (String.Equals(potentialChildPath, potentialParentPath, StringComparison.Ordinal))
                return alsoReturnTrueIfSamePath;

            if (potentialChildPath.StartsWith(potentialParentPath, StringComparison.Ordinal))
            {
                // potentialChildPath[potentialParentPath.Length] is guaranteed to not throw an IndexOutOfRangeException. If the string was too short we would not be here
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
        /// This function iterates through a directory's child FileSystemInfos to find the last write time of any info [file or directory] within.
        /// </summary>
        public static DateTime GetDirectoryLastWriteTimeIncludingChildren(string directoryPath)
        {
            if (!Directory.Exists(directoryPath))
                throw new DirectoryNotFoundException($"Couldn't find directory {directoryPath}");

            var directory = new DirectoryInfo(directoryPath);
            var latestTime = directory.LastWriteTime;

            foreach (var info in directory.EnumerateFileSystemInfos("*", SearchOption.AllDirectories))
            {
                if (info.LastWriteTime > latestTime)
                    latestTime = info.LastWriteTime;
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
        /// <param name="prefix">The directory name will be prefixed with this, if you supply it. Useful for debugging.</param>
        public static string CreateTemporaryDirectory(string prefix = "")
        {
            var directoryPath = Path.Combine(Path.GetTempPath(), prefix + JRandom.Shared.UppercaseAlphanumericString(20));
            directoryPath = MakeProposedDirectoryPathUnique(directoryPath); // Just in case lol
            Directory.CreateDirectory(directoryPath);
            return directoryPath;
        }


        public static bool FileContentsAreIdentical(string filePath1, string filePath2)
        {
            if (string.IsNullOrEmpty(filePath1))
                throw new ArgumentException($"{nameof(filePath1)} cannot be null or empty.", nameof(filePath1));

            if (string.IsNullOrEmpty(filePath2))
                throw new ArgumentException($"{nameof(filePath2)} cannot be null or empty.", nameof(filePath2));

            if (!FileUtilities.IsLegalFileSystemPath(filePath1))
                throw new ArgumentException($"{nameof(filePath1)} is illegal: {filePath1}", nameof(filePath1));

            if (!FileUtilities.IsLegalFileSystemPath(filePath2))
                throw new ArgumentException($"{nameof(filePath2)} is illegal: {filePath2}", nameof(filePath2));


            var fileInfo1 = new FileInfo(filePath1);
            var fileInfo2 = new FileInfo(filePath2);


            if (!fileInfo1.Exists)
                throw new FileNotFoundException($"{nameof(fileInfo1)} does not exist or could not be found: {fileInfo1}");

            if (!fileInfo2.Exists)
                throw new FileNotFoundException($"{nameof(fileInfo2)} does not exist or could not be found: {fileInfo2}");


            if (fileInfo1.Length != fileInfo2.Length)
                return false;

            if (fileInfo1.FullName == fileInfo2.FullName)
                return true;

            // Todo: determine whether this is actually a performance increase, enable it if so
            //if (CryptographyUtility.GetFileSHA1(filePath1) != CryptographyUtility.GetFileSHA1(filePath2))
            //    return false;


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