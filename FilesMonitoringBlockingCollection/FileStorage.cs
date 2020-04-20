using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FilesMonitoringBlockingCollection
{
    static class FileStorage
    {
        public static BlockingCollection<string> FileCollection = new BlockingCollection<string>(4);

        public static Dictionary<string, string> ProcessedFiles = new Dictionary<string, string>();
    }
}
