using System;
using System.IO;

namespace indexer.common.Models
{
    public class FileEntry
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string FullName { get; set; }
        public string Extension { get; set; }
        public long? Size { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastAccessTime { get; set; }
        public DateTime? LastWriteTime { get; set; }
        public DateTime IndexedDate { get; set; }
    }
}