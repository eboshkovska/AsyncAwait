using FilesMonitoringAsyncAwait;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FilesMonitoingAsyncAwait
{
    class Program
    {
        private static int maxNumberOfProcessedFiles = 10;
        static AutoResetEvent waitHandle = new AutoResetEvent(false);
        static CancellationTokenSource tokenSource = new CancellationTokenSource();

        static async Task Main(string[] args)
        {
            var publisher = new FilePublisher();
            var consumer = new FilesConsumer();

            publisher.StartMonitoring();

            do {
                await consumer.ConsumeAsync();

                if (FileStorage.ProcessedFiles.Count == maxNumberOfProcessedFiles)
                {
                    tokenSource.Cancel();
                    waitHandle.Set();
                }
            } while (!tokenSource.Token.IsCancellationRequested);
            
            PrintResults();

            waitHandle.WaitOne();
        }

        private static void PrintResults()
        {
            foreach (KeyValuePair<string, string> file in FileStorage.ProcessedFiles)
            {
                Console.WriteLine($"{file.Key} - {file.Value}");
            }
        }
    }
}
