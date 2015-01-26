using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoManager
{
    public class FileIndexItem
    {
        public string FullPath { get; set; }

        public string FileName { get; set; }

        public DateTime FileCreationTime { get; set; }

        public string Extension { get; set; }

        public long FileSize { get; set; }

        public DateTime? OriginalDateTaken { get; set; }
    }
}
