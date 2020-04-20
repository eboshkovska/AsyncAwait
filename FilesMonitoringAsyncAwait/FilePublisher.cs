using System.IO;

namespace FilesMonitoringAsyncAwait
{
    class FilePublisher
    {
        private FileSystemWatcher watcher;

        public FilePublisher()
        {
            watcher = new FileSystemWatcher();
        }

        public void StartMonitoring()
        {
            watcher.Path = @"C:\Files";
           
            watcher.Created += OnCreated;

            watcher.EnableRaisingEvents = true;
        }

        void OnCreated(object source, FileSystemEventArgs e) => FileStorage.FileQueue.Enqueue(e.FullPath);
    }
}
