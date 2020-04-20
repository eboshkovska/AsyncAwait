using System.IO;
using System.Threading;

namespace FilesMonitoringBlockingCollection
{
    class FilesConsumer
    {
        private CountdownEvent countdownEvent;
        private CancellationToken token;

        public FilesConsumer(CountdownEvent countdownEvent, CancellationToken token)
        {
            this.countdownEvent = countdownEvent;
            this.token = token;
        }

       
        public void Consume()
        {
            while (!token.IsCancellationRequested)
            {
                lock (FileStorage.FileCollection)
                {
                    if (FileStorage.FileCollection.Count > 0)
                    {
                        FileStorage.FileCollection.TryTake(out var filePath);

                        try
                        {
                            var fileContent = File.ReadAllText(filePath);
                            var fileName = Path.GetFileName(filePath);

                            FileStorage.ProcessedFiles.Add(fileName, fileContent);

                            this.countdownEvent.Signal();
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
