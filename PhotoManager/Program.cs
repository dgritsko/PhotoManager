using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;

namespace PhotoManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
        }

        public static void BuildIndex(string baseDir, string outputPath, string description)
        {
            var files = GetFileNames(baseDir, true)
                .AsParallel()
                .Where(x => !ShouldIgnoreFile(x))
                .Select(CreateFileIndexItem)
                .ToList();

            var directory = new DirectoryIndexItem()
            {
                Path = baseDir,
                Files = files,
                Description = description
            };

            var json = JsonConvert.SerializeObject(directory, Formatting.Indented);

            File.WriteAllText(outputPath, json);
        }

        public static bool ShouldIgnoreFile(string filePath)
        {
            if (filePath.EndsWith(".lnk", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            
            if (filePath.EndsWith(".ini", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        public class DirectoryIndexItem
        {
            public List<FileIndexItem> Files { get; set; }

            public string Path { get; set; }

            public string Description { get; set; }
        }

        public static FileIndexItem CreateFileIndexItem(string fullPath)
        {
            var fileInfo = new System.IO.FileInfo(fullPath);

            var originalDateTaken = GetOriginalDateTaken(fullPath);

            return new FileIndexItem
            {
                FullPath = fullPath,
                FileName = fileInfo.Name,
                FileCreationTime = fileInfo.CreationTime,
                OriginalDateTaken = originalDateTaken,
                Extension = fileInfo.Extension,
                FileSize = fileInfo.Length,
            };
        }

        public static DateTime? GetOriginalDateTaken(string fullPath)
        {
            try
            {
                var file = TagLib.File.Create(fullPath);

                var imageFile = file as TagLib.Image.File;
                if (imageFile == null)
                {
                    // Not an image file
                    return null;
                }

                return imageFile.ImageTag.DateTime;
            }
            catch
            {
                return null;
            }
        }

        public class FileIndexItem
        {
            public string FullPath { get; set; }

            public string FileName { get; set; }

            public DateTime FileCreationTime { get; set; }

            public string Extension { get; set; }

            public long FileSize { get; set; }

            public DateTime? OriginalDateTaken { get; set; }
        }

        public static IEnumerable<string> GetFileNames(string baseDir, bool recursive)
        {
            var files = Directory.GetFiles(baseDir);

            var count = 0;

            foreach (var file in files)
            {
                yield return file;

                count++;
            }

            Console.WriteLine(string.Format("Found {0} files in {1}", count, baseDir));

            if (recursive)
            {
                var subDirectories = Directory.GetDirectories(baseDir);

                foreach (var subDirectory in subDirectories)
                {
                    foreach (var fileInSubDir in GetFileNames(subDirectory, recursive))
                    {
                        yield return fileInSubDir;
                    }
                }
            }
        }
    }
}
