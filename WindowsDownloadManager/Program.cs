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
                ".sh", ".script", ".ps1", ".ba_", ".prg", ".osx", ".csh",  };

            int fileCount = downloads.GetFiles().Length;
            foreach(FileInfo file in downloads.GetFiles())
            {
                if (file.Name == "desktop.ini") 
                {
                    fileCount--;
                    continue; 
                }
                Console.WriteLine(file);
                if (docExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!documentDir.Exists) { documentDir.Create(); }
                    file.MoveTo($@"C:\Users\adamb\Downloads\Documents\{file.Name}");
                }
                if (videoExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!videoDir.Exists) { videoDir.Create(); }
                    file.MoveTo($@"C:\Users\adamb\Downloads\Videos\{file.Name}");
                }
                if (audioExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!audioDir.Exists) { audioDir.Create(); }
                    file.MoveTo($@"C:\Users\adamb\Downloads\Audio\{file.Name}");
                }
                if (pictExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!pictureDir.Exists) { pictureDir.Create(); }
                    file.MoveTo($@"C:\Users\adamb\Downloads\Pictures\{file.Name}");
                }
                if (execExts.Any(x => file.Extension.ToLower() == x))
                {
                    if (!executableDir.Exists) { executableDir.Create(); }
                    file.MoveTo($@"C:\Users\adamb\Downloads\Executables\{file.Name}");
                }
                if (file.Extension.ToLower() == ".zip")
                {
                    if (!zipDir.Exists) { zipDir.Create(); }
                    file.MoveTo($@"C:\Users\adamb\Downloads\Zips\{file.Name}");
                }
            }
            Console.WriteLine($"Moved {fileCount} file(s).");
            Console.WriteLine("WindowsDownloadManager Complete.");
            Console.ReadLine();
        }
    }
}
