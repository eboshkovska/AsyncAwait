using System;
using System.Collections.Generic;
using System.Threading;

namespace FilesMonitoringThreads
{
    class Program
    {       
        static AutoResetEvent waitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            var countDownEvent = new CountdownEvent(10);

            var publisher = new FilePublisher();
            var consumer = new FilesConsumer(countDownEvent);

            new Thread(publisher.StartMonitoring).Start();

            TimerCallback timerCallBack = new TimerCallback(consumer.Consume);
            Timer timer = new Timer(timerCallBack, null, 0, 2000);
            
            countDownEvent.Wait();

            timer.Dispose();

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
