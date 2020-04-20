using System;
using System.IO;
using System.Threading;

namespace FilesMonitoringTasks
{
    class FilesConsumer
    {
        private CountdownEvent countdownEvent;
        private CancellationToken token;
        private SemaphoreSlim semaphore; 

        public FilesConsumer(CountdownEvent countdownEvent, CancellationToken token)
        {
            this.countdownEvent = countdownEvent;
            this.token = token;
            semaphore = new SemaphoreSlim(4);
        }

        public void Consume()
        {
            while (!token.IsCancellationRequested)
            {
                lock (FileStorage.FileQueue)
                {
                    if (FileStorage.FileQueue.Count > 0)
                    {
                        semaphore.Wait();

                        var filePath = FileStorage.FileQueue.Dequeue();
                        try
                        {
                            var fileContent = File.ReadAllText(filePath);
                            var fileName = Path.GetFileName(filePath);

                            FileStorage.ProcessedFiles.Add(fileName, fileContent);
                                                        
                            this.countdownEvent.Signal();
                        }
                        catch { }
                        semaphore.Release();
                    }
                }
            }
        }
    }
}
