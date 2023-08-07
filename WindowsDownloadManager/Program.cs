using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsDownloadManager
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.SetWindowPosition(0, 0);
            Console.SetCursorPosition(0, 0);

            DirectoryInfo downloads = new DirectoryInfo(@"C:\Users\adamb\Downloads");
            DirectoryInfo documentDir = new DirectoryInfo(@"C:\Users\adamb\Downloads\Documents");
            DirectoryInfo videoDir = new DirectoryInfo(@"C:\Users\adamb\Downloads\Videos");
            DirectoryInfo audioDir = new DirectoryInfo(@"C:\Users\adamb\Downloads\Audio");
            DirectoryInfo pictureDir = new DirectoryInfo(@"C:\Users\adamb\Downloads\Pictures");
            DirectoryInfo executableDir = new DirectoryInfo(@"C:\Users\adamb\Downloads\Executables");
            DirectoryInfo zipDir = new DirectoryInfo(@"C:\Users\adamb\Downloads\Zips_and_Dirs");

            string[] docExts = { ".pdf", ".doc", ".docx", ".txt" };
            string[] videoExts = { ".mp4", ".flv", ".m4v", "mpg", "mpeg", "amv", ".mov", ".avi", ".gifv", ".webm" };
            string[] audioExts = { ".mp3", ".flac", ".wav", ".aiff", ".m4a", ".wma", ".aac" };
            string[] pictExts = { ".png", ".jpg", ".jpeg", ".gif", ".svg", ".psd" };
            string[] execExts = { ".exe", ".msi", ".jar", ".bat", ".cmd", ".run", ".bin", ".app", ".x86", ".com", 
                ".sh", ".script", ".ps1", ".ba_", ".prg", ".osx", ".csh" };
            string[] allowedDirs = { "Audio", "Videos", "Documents", "Pictures", "Executables", "Zips_and_Dirs" };
            
            int fileCount = 0;
            int dirCount = 0;

            foreach(FileInfo file in downloads.GetFiles())
            {
                if (file.Name == "desktop.ini") 
                {
                    continue; 
                }
                if (docExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!documentDir.Exists) { documentDir.Create(); }
                    fileCount += MoveFile(file, $@"{documentDir}\");
                }
                if (videoExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!videoDir.Exists) { videoDir.Create(); }
                    fileCount += MoveFile(file, $@"{videoDir}\");
                }
                if (audioExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!audioDir.Exists) { audioDir.Create(); }
                    fileCount += MoveFile(file, $@"{audioDir}\");
                }
                if (pictExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!pictureDir.Exists) { pictureDir.Create(); }
                    fileCount += MoveFile(file, $@"{pictureDir}\");
                }
                if (execExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!executableDir.Exists) { executableDir.Create(); }
                    fileCount += MoveFile(file, $@"{executableDir}\");
                }
                if (file.Extension.ToLower() == ".zip")
                {
                    if (!zipDir.Exists) { zipDir.Create(); }
                    fileCount += MoveFile(file, $@"{zipDir}\");
                }
            }

            foreach (DirectoryInfo dir in downloads.GetDirectories())
            {
                if (!allowedDirs.Any(d => dir.Name == d))
                {
                    dirCount += MoveDir(dir, $@"{zipDir}\");
                }
            }

            Console.WriteLine($"Organized {fileCount} file(s) and {dirCount} directory/directories");
            Console.WriteLine("WindowsDownloadManager Complete.");
            Console.ReadLine();
        }

        private static int MoveFile(FileInfo fInfo, string destinationPath)
        {
            try
            {
                fInfo.MoveTo($@"{destinationPath}{fInfo.Name}");
                Console.WriteLine($@"Moved file - {fInfo.FullName} to [{destinationPath}{fInfo.Name}]");
                return 1;
            }
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

        private static int MoveDir(DirectoryInfo dInfo, string destinationPath)
        {
            try
            {
                dInfo.MoveTo($@"{destinationPath}{dInfo.Name}");
                Console.WriteLine($@"Moved directory - {dInfo.FullName} to [{destinationPath}{dInfo.Name}]");
                return 1;
            }
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
    }
}
