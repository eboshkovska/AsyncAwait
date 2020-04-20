using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FilesMonitoringTasks
{
    class Program
    {
        static AutoResetEvent waitHandle = new AutoResetEvent(false);
        static CancellationTokenSource tokenSource = new CancellationTokenSource();
        static CancellationToken token = tokenSource.Token;

        static void Main(string[] args)
        {
            var countDownEvent = new CountdownEvent(10);

            var publisher = new FilePublisher();
            var consumer = new FilesConsumer(countDownEvent, token);

            Task.Factory.StartNew(() =>
            {
                publisher.StartMonitoring();

            }, token);

            var task = Task.Factory.StartNew(() =>
            {
                consumer.Consume();

            }, token);

            countDownEvent.Wait();

            tokenSource.Cancel();

            PrintResults();

            waitHandle.Set();

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
