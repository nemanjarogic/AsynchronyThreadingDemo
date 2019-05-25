using System;
using System.ComponentModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronyThreadingDemo.Demo
{
    public class ThreadPooling
    {
        #region Fields

        private BackgroundWorker _backgroundWorker;

        #endregion Fields

        #region QueueUserWorkItem methods

        /// <summary>
        /// QueueUserWorkItem doesn't provide easy mechanism for getting return values back from a thread
        /// Used if we are targeting a .NET Framework version prior 4.0
        /// </summary>
        public void EnterTPViaQueueUserWorkItem()
        {
            ThreadPool.QueueUserWorkItem(PrintToConsole);
            ThreadPool.QueueUserWorkItem(PrintToConsole, "Parameter is passed");
        }

        private void PrintToConsole(object data)
        {
            Console.WriteLine("Hello from the thread pool! " + data);
        }

        #endregion QueueUserWorkItem methods

        #region Asynchronous delegates methods

        /// <summary>
        /// Asynchronous delegates solve problem with getting return values back from a thread
        /// </summary>
        public void EnterTPViaAsynchronousDelegates()
        {
            Func<string, int> method = GetLength;
            IAsyncResult asyncResult = method.BeginInvoke("Parameter", null, null);

            // Here's where we can do other work in parallel...
            Console.WriteLine("Executing is continued...");

            int result = method.EndInvoke(asyncResult);
            Console.WriteLine($"Calculated string length is: {result}");
        }

        private int GetLength(string str)
        {
            Console.WriteLine($"Calculating string length for '{str}'...");
            return str.Length;
        }

        #endregion Asynchronous delegates methods

        #region Task parallel library methods

        public void EnterTPViaTPL()
        {
            Console.WriteLine("Entering thread pool via Task Parallel Library (TPL) using nongeneric Task...");
            Task.Factory.StartNew(() => PrintContentToConsole("Dummy content"));
        }

        public void EnterTPViaTPLWithResult()
        {
            string uri = "https://docs.microsoft.com/en-us/dotnet/standard/threading/threads-and-threading";

            Console.WriteLine("Entering thread pool via Task Parallel Library (TPL) using Task<T>...");
            Console.WriteLine($"Trying to download content from {uri}");

            // Start task executing
            Task<string> task = Task.Factory.StartNew(() => DownloadWebContent(uri));

            // We can do other work here and it will execute in parallel
            Console.WriteLine("Executing is continued...");

            // When we need the task's return value, we query its Result property:
            // If it's still executing, the current thread will now block (wait)
            // until the task finishes:
            PrintContentToConsole(task.Result);
        }

        private string DownloadWebContent(string uri)
        {
            string content = string.Empty;
            using (WebClient wc = new WebClient())
            {
                content = wc.DownloadString(uri);
            }

            return content;
        }

        private void PrintContentToConsole(string content)
        {
            Console.WriteLine($"Content for printing: {content}");
        }

        #endregion Task parallel library methods

        #region BackgroundWorker methods

        /// <summary>
        /// With BackgroundWorker we have ability to safely update WPF or Windows Form controls when the worker completes
        /// BackgroundWorker has a protocol for reporting progress
        /// </summary>
        public void EnterTPViaBackgroundWorker()
        {
            _backgroundWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _backgroundWorker.DoWork += DoWork;
            _backgroundWorker.ProgressChanged += ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += RunWorkerCompleted;

            _backgroundWorker.RunWorkerAsync("Hello to worker");

            Console.WriteLine("Press Enter in the next 5 seconds to cancel operation...");
            Console.ReadLine();
            if (_backgroundWorker.IsBusy)
            {
                _backgroundWorker.CancelAsync();
            }
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("BackgroundWorker started...");

            for (int index = 0; index <= 100; index += 20)
            {
                if (_backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                _backgroundWorker.ReportProgress(index);

                // Simulate some work for the demo
                Thread.Sleep(1000);      
            }

            // This gets passed to RunWorkerCompleted
            e.Result = 33;    
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine($"Reached {e.ProgressPercentage} %");
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("User canceled BackgroundWorker operation!");
            }
            else if (e.Error != null)
            {
                Console.WriteLine("BackgroundWorker exception: " + e.Error.ToString());
            }
            else
            {
                // Result is set in DoWork method
                Console.WriteLine("BackgroundWorker completed with result: " + e.Result);     
            }
        }

        #endregion BackgroundWorker methods
    }
}
