using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoManager
{
    public static class IndexCondenser
    {
        public static IEnumerable<FileIndexItem> CleanupList(IEnumerable<List<FileIndexItem>> files)
        {
            foreach (var file in files)
            {
                if (file.Count == 1)
                {
                    yield return file.FirstOrDefault();
                }
                else if (file.Select(x => x.OriginalDateTaken).Distinct().Count() == 1 && file.Select(x => x.FileSize).Distinct().Count() == 1)
                {
                    yield return file.FirstOrDefault();
                }
                else
                {
                    throw new ArgumentException("files");
                }
            }
        }

        public static IEnumerable<List<FileIndexItem>> CombineIndexes(IEnumerable<DirectoryIndexItem> directories)
        {
            var files = new Dictionary<string, List<FileIndexItem>>();

            foreach (var directory in directories)
            {
                foreach (var file in directory.Files)
                {
                    var canonicalFilename = file.FileName.ToLower();

                    if (!files.ContainsKey(canonicalFilename))
                    {
                        files[canonicalFilename] = new List<FileIndexItem>() { file };
                    }
                    else
                    {
                        files[canonicalFilename].Add(file);
                    }
                }
            }

            return files.Values;
        }
    }
}
