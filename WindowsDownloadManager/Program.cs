using System;
using System.IO;
using System.Linq;

namespace WindowsDownloadManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Set console window.
            Console.Clear();
            Console.SetWindowPosition(0, 0);
            Console.SetCursorPosition(0, 0);
            Console.SetWindowSize(170, 30);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Windows Downloads Manager Starting: [{DateTime.Now}]");

            // Get needed directories.
            DirectoryInfo downloadDir = new DirectoryInfo($@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\\Downloads");
            DirectoryInfo documentDir = new DirectoryInfo($@"{downloadDir.FullName}\Documents");
            DirectoryInfo videoDir = new DirectoryInfo($@"{downloadDir.FullName}\Videos");
            DirectoryInfo audioDir = new DirectoryInfo($@"{downloadDir.FullName}\Audio");
            DirectoryInfo pictureDir = new DirectoryInfo($@"{downloadDir.FullName}\Pictures");
            DirectoryInfo executableDir = new DirectoryInfo($@"{downloadDir.FullName}\Executables");
            DirectoryInfo zipDir = new DirectoryInfo($@"{downloadDir.FullName}\Zips_and_Dirs");

            // Different document extension types.
            string[] docExts = { ".pdf", ".doc", ".docx", ".txt", ".odt" };
            string[] videoExts = { ".mp4", ".flv", ".m4v", "mpg", "mpeg", "amv", ".mov", ".avi", ".gifv", ".webm" };
            string[] audioExts = { ".mp3", ".flac", ".wav", ".aiff", ".m4a", ".wma", ".aac" };
            string[] pictExts = { ".png", ".jpg", ".jpeg", ".gif", ".svg", ".psd" };
            string[] execExts = { ".exe", ".msi", ".jar", ".bat", ".cmd", ".run", ".bin", ".app", ".x86", ".com", 
                ".sh", ".script", ".ps1", ".ba_", ".prg", ".osx", ".csh" };
            string[] allowedDirs = { "Audio", "Videos", "Documents", "Pictures", "Executables", "Zips_and_Dirs" };
            
            int fileCount = 0;
            int dirCount = 0;
            long bytesRunningTotal = 0;
            string bytesMoved;

            foreach(FileInfo file in downloadDir.GetFiles())
            {
                if (file.Name == "desktop.ini") 
                {
                    continue; 
                }
                if (docExts.Any(x => file.Extension.ToLower() == x))
                {
                    CreateDir(documentDir);
                    int fileMoved = MoveFile(file, $@"{documentDir}\");
                    fileCount += fileMoved;
                    bytesRunningTotal += fileMoved > 0 ? file.Length : 0;
                }
                if (videoExts.Any(x => file.Extension.ToLower() == x))
                {
                    CreateDir(videoDir);
                    int fileMoved = MoveFile(file, $@"{videoDir}\");
                    fileCount += fileMoved;
                    bytesRunningTotal += fileMoved > 0 ? file.Length : 0;
                }
                if (audioExts.Any(x => file.Extension.ToLower() == x))
                {
                    CreateDir(audioDir);
                    int fileMoved = MoveFile(file, $@"{audioDir}\");
                    fileCount += fileMoved;
                    bytesRunningTotal += fileMoved > 0 ? file.Length : 0;
                }
                if (pictExts.Any(x => file.Extension.ToLower() == x))
                {
                    CreateDir(pictureDir);
                    int fileMoved = MoveFile(file, $@"{pictureDir}\");
                    fileCount += fileMoved;
                    bytesRunningTotal += fileMoved > 0 ? file.Length : 0;
                }
                if (execExts.Any(x => file.Extension.ToLower() == x))
                {
                    CreateDir(executableDir);
                    int fileMoved = MoveFile(file, $@"{executableDir}\");
                    fileCount += fileMoved;
                    bytesRunningTotal += fileMoved > 0 ? file.Length : 0;
                }
                if (file.Extension.ToLower() == ".zip")
                {
                    CreateDir(zipDir);
                    int fileMoved = MoveFile(file, $@"{zipDir}\");
                    fileCount += fileMoved;
                    bytesRunningTotal += fileMoved > 0 ? file.Length : 0;
                }
            }

            foreach (DirectoryInfo dir in downloadDir.GetDirectories())
            {
                if (!allowedDirs.Any(d => dir.Name == d))
                {
                    dirCount += MoveDir(dir, $@"{zipDir}\");
                }
            }

            bytesMoved = FormatBytes(bytesRunningTotal);

            Console.WriteLine($"\nOrganized {fileCount} file(s) and {dirCount} directory/directories | {bytesMoved}");
            Console.WriteLine($"Windows Downloads Manager Finished: [{DateTime.Now}]");
            Console.ReadLine();
        }

        /// <summary>
        /// This method moves the provided file to the given location.
        /// </summary>
        /// <param name="fInfo">The file to move.</param>
        /// <param name="destinationPath">The new location of the file.</param>
        /// <returns>An integer representing whether the file was fileMoved or not.</returns>
        private static int MoveFile(FileInfo fInfo, string destinationPath)
        {
            try
            {
                fInfo.MoveTo($@"{destinationPath}{fInfo.Name}");
                Console.WriteLine($@"Moved file - {fInfo.FullName} to [{destinationPath}{fInfo.Name}]");
                return 1;
            }
            // File already exists in target location - overwrite.
            catch (IOException)
            {
                Console.WriteLine($@"Overwriting - [{destinationPath}{fInfo.Name}] with [{fInfo.FullName}]");
                fInfo.CopyTo($@"{destinationPath}{fInfo.Name}", true);
                fInfo.Delete();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// This method moves the provided directory to the given location.
        /// </summary>
        /// <param name="dInfo">The directory to move.</param>
        /// <param name="destinationPath">The new location of the directory.</param>
        /// <returns>An integer representing whether the directory was fileMoved or not.</returns>
        private static int MoveDir(DirectoryInfo dInfo, string destinationPath)
        {
            try
            {
                dInfo.MoveTo($@"{destinationPath}{dInfo.Name}");
                Console.WriteLine($@"Moved directory - {dInfo.FullName} to [{destinationPath}{dInfo.Name}]");
                return 1;
            }
            // Directory already exists in target location - replace.
            catch (IOException)
            {
                Console.WriteLine($@"Overwriting directory - [{destinationPath}{dInfo.Name}] with [{dInfo.FullName}]");
                DirectoryInfo dirToDelete = new DirectoryInfo($@"{destinationPath}{dInfo.Name}");
                dirToDelete.Delete();
                dInfo.MoveTo($@"{destinationPath}{dInfo.Name}");
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        /// <summary>
        /// This method will create a directory if needed.
        /// </summary>
        /// <param name="newDir">The directory name to check and create.</param>
        private static void CreateDir(DirectoryInfo newDir)
        {
            if (newDir != null && !newDir.Exists)
            {
                newDir.Create();
                Console.WriteLine($@"Created missing directory: {newDir.FullName}");
            }
        }

        /// <summary>
        /// Calculates the human readable size of bytes.
        /// </summary>
        /// <param name="length">The number of bytes</param>
        /// <returns>A formatted string representation of the total byte size.</returns>
        private static string FormatBytes(long length)
        {
            long B, KB = 1024, MB = KB * 1024, GB = MB * 1024, TB = GB * 1024;
            double size = length;
            string suffix = nameof(B);

            if (length >= TB)
            {
                size = Math.Round((double)length / TB, 2);
                suffix = nameof(TB);
            }
            else if (length >= GB)
            {
                size = Math.Round((double)length / GB, 2);
                suffix = nameof(GB);
            }
            else if (length >= MB)
            {
                size = Math.Round((double)length / MB, 2);
                suffix = nameof(MB);
            }
            else if (length >= KB)
            {
                size = Math.Round((double)length / KB, 2);
                suffix = nameof(KB);
            }

            return $"{size} {suffix}";
        }
    }
}
