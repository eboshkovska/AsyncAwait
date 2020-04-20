﻿using System;
using System.Collections.Generic;

namespace FilesMonitoringThreads
{
    static class FileStorage
    {
        public static Queue<string> FileQueue = new Queue<string>();
        
        public static Dictionary<string, string> ProcessedFiles = new Dictionary<string, string>();
    }
}
