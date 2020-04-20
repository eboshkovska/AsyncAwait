using FilesMonitoringAsyncAwait;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FilesMonitoingAsyncAwait
{
    class FilesConsumer
    {
        private SemaphoreSlim semaphore;

        public FilesConsumer()
        {           
            semaphore = new SemaphoreSlim(4);
        }

        public Task ConsumeAsync()
        {
            return Task.Factory.StartNew(async () =>
            {
                if (FileStorage.FileQueue.Count > 0)
                {
                    semaphore.Wait();

                    var filePath = FileStorage.FileQueue.Dequeue();
                    try
                    {
                        var fileContent = await File.ReadAllTextAsync(filePath);
                        var fileName = Path.GetFileName(filePath);

                        FileStorage.ProcessedFiles.Add(fileName, fileContent);
                    }
                    catch { }
                    semaphore.Release();
                }
            });
        }
    }
}
