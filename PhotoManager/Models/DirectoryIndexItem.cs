using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoManager.Models
{
    public class DirectoryIndexItem
    {
        public List<FileIndexItem> Files { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }
    }
}
