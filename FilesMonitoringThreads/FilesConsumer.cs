using System;
using System.IO;
using System.Text;
using System.Threading;

namespace FilesMonitoringThreads
{
    class FilesConsumer
    {
        private CountdownEvent countdownEvent;

        public FilesConsumer(CountdownEvent countdownEvent)
        {
            this.countdownEvent = countdownEvent;
        }

        public void Consume(object stateObj)
        {
            lock (FileStorage.FileQueue)
            {
                if (FileStorage.FileQueue.Count > 0)
                {
                    var filePath = FileStorage.FileQueue.Dequeue();
                    try
                    {
                        if (FileStorage.ProcessedFiles.Count + 1 <= 10)
                        {
                            var fileContent = File.ReadAllText(filePath);
                            var fileName = Path.GetFileName(filePath);

                            FileStorage.ProcessedFiles.Add(fileName, fileContent);

                            this.countdownEvent.Signal();
                        }
                    }
                    catch { }
                }
            }
        }
    }
}
