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
            DirectoryInfo zipDir = new DirectoryInfo(@"C:\Users\adamb\Downloads\Zips");

            string[] docExts = { ".pdf", ".doc", ".docx", ".txt" };
            string[] videoExts = { ".mp4", ".flv", ".m4v", "mpg", "mpeg", "amv", ".mov", ".avi", ".gifv", ".webm" };
            string[] audioExts = { ".mp3", ".flac", ".wav", ".aiff", ".m4a", ".wma", ".aac" };
            string[] pictExts = { ".png", ".jpg", ".jpeg", ".gif", ".svg", ".psd" };
            string[] execExts = { ".exe", ".msi", ".jar", ".bat", ".cmd", ".run", ".bin", ".app", ".x86", ".com", 
                ".sh", ".script", ".ps1", ".ba_", ".prg", ".osx", ".csh" };

            int fileCount = 0;
            foreach(FileInfo file in downloads.GetFiles())
            {
                if (file.Name == "desktop.ini") 
                {
                    continue; 
                }
                if (docExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!documentDir.Exists) { documentDir.Create(); }
                    fileCount += MoveFile(file, @"C:\Users\adamb\Downloads\Documents\");
                }
                if (videoExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!videoDir.Exists) { videoDir.Create(); }
                    fileCount += MoveFile(file, @"C:\Users\adamb\Downloads\Videos\");
                    
                }
                if (audioExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!audioDir.Exists) { audioDir.Create(); }
                    fileCount += MoveFile(file, @"C:\Users\adamb\Downloads\Audio\");
                }
                if (pictExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!pictureDir.Exists) { pictureDir.Create(); }
                    fileCount += MoveFile(file, @"C:\Users\adamb\Downloads\Pictures\");
                }
                if (execExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!executableDir.Exists) { executableDir.Create(); }
                    fileCount += MoveFile(file, @"C:\Users\adamb\Downloads\Executables\");
                }
                if (file.Extension.ToLower() == ".zip")
                {
                    if (!zipDir.Exists) { zipDir.Create(); }
                    fileCount += MoveFile(file, @"C:\Users\adamb\Downloads\Zips\");
                }
            }
            Console.WriteLine($"Organized {fileCount} file(s).");
            Console.WriteLine("WindowsDownloadManager Complete.");
            Console.ReadLine();
        }

        private static int MoveFile(FileInfo fInfo, String destinationPath)
        {
            try
            {
                fInfo.MoveTo($@"{destinationPath}{fInfo.Name}");
                Console.WriteLine($@"Moved {fInfo.Name} to [{destinationPath}{fInfo.Name}]");
                return 1;
            }
            catch (IOException)
            {
                Console.WriteLine($@"Overwriting [{destinationPath}{fInfo.Name}] with [{fInfo.FullName}]");
                fInfo.CopyTo($@"{destinationPath}{fInfo.Name}", true);
                fInfo.Delete();
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
