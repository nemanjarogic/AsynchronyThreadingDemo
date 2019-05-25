using AsynchronyThreadingDemo.AsynchronousPatterns;
using AsynchronyThreadingDemo.Attention;
using AsynchronyThreadingDemo.Demo;
using AsynchronyThreadingDemo.Proxy;
using System;
using System.IO;

namespace AsynchronyThreadingDemo
{
    class Program
    {
        #region Fields

        private static Lazy<ThreadPooling> _threadPooling = new Lazy<ThreadPooling>(() => new ThreadPooling(), false);
        private static Lazy<ExceptionHandler> _exceptionHandler = new Lazy<ExceptionHandler>(() => new ExceptionHandler(), false);

        #endregion Fields

        #region Properties

        public static ThreadPooling LazyThreadPooling
        {
            get
            {
                return _threadPooling.Value;
            }
        }

        public static ExceptionHandler LazyExceptionHandler
        {
            get
            {
                return _exceptionHandler.Value;
            }
        }

        #endregion Properties

        #region Menu

        static void Main(string[] args)
        {
            bool isExecutionAborted = false;
            while (!isExecutionAborted)
            {
                ListConsoleMenu();
                string request = Console.ReadLine();

                if (Int32.TryParse(request, out int requestNumber))
                {
                    switch (requestNumber)
                    {
                        case 1:
                            EnterTPViaQueueUserWorkItem(); break;
                        case 2:
                            EnterTPViaAsynchronousDelegates(); break;
                        case 3:
                            EnterTPViaTaskParallelLibrary(); break;
                        case 4:
                            EnterTPViaBackgroundWorker(); break;
                        case 5:
                            ExecuteWrongThreadHandlingPrior(); break;
                        case 6:
                            ExecuteThreadHandlingPrior(); break;
                        case 7:
                            ExecuteCallerHandlingPrior(); break;
                        case 8:
                            ExecuteThreadHandlingAbove(); break;
                        case 9:
                            ExecuteCallerHandlingAbove(); break;
                        case 10:
                            ExecuteDelayWithTCS(); break;
                        case 11:
                            ExecuteSdkMockWithTCS(); break;
                        case 12:
                            DemonstrateAsynchronousProgrammingModelPattern(); break;
                        case 13:
                            DemonstrateEventBasedAsynchronousPattern(); break;
                        case 14:
                            DemonstrateTaskBasedAsynchronousPattern(); break;
                        case 15:
                            CompareConcurentDictionaryWithLock(); break;
                        case 100:
                            DemonstrateCapturedVariablesProblem(); break;

                        case 0:
                            isExecutionAborted = true;
                            break;
                        default:
                            break;
                    }
                }
            }

            Console.ReadLine();
        }

        protected static void ListConsoleMenu()
        {
            Console.WriteLine("\n\n---------------------------------------------------------------------------------");
            Console.WriteLine("Please select an option: ");
            Console.WriteLine("---------------------------------------------------------------------------------\n");

            Console.WriteLine("--- 1. Enter thread pool via ThreadPool.QueueUserWorkItem ---");
            Console.WriteLine("--- 2. Enter thread pool via asynchronous delegates ---");
            Console.WriteLine("--- 3. Enter thread pool via Task Parallel Library ---");
            Console.WriteLine("--- 4. Enter thread pool via BackgroundWorker ---");
            Console.WriteLine("--- 5. Execute bad thread exception handling [prior .NET Framework 4] ---");
            Console.WriteLine("--- 6. Execute thread exception handling [prior .NET Framework 4] ---");
            Console.WriteLine("--- 7. Execute caller exception handling [prior .NET Framework 4] ---");
            Console.WriteLine("--- 8. Execute thread exception handling [from .NET Framework  4] ---");
            Console.WriteLine("--- 9. Execute caller exception handling [from .NET Framework  4] ---");
            Console.WriteLine("--- 10. Demonstrate Task.Delay with Task Completion Source ---");
            Console.WriteLine("--- 11. Demonstrate using of third-party SDK with Task Completion Source ---");
            Console.WriteLine("--- 12. Demonstrate Asynchronous Programming Model pattern (APM) ---");
            Console.WriteLine("--- 13. Demonstrate Event-based Asynchronous Pattern (EAP) ---");
            Console.WriteLine("--- 14. Demonstrate Task-based Asynchronous Pattern (TAP) ---");
            Console.WriteLine("--- 15. Compare ConcurrentDictionary vs. lock performance ---");

            Console.WriteLine("\n--- 100. Demonstrate captured variables problem ---");

            Console.WriteLine("\n--- 0. Exit ---\n\n");
        }

        #endregion Menu

        #region Thread pooling methods

        private static void EnterTPViaQueueUserWorkItem()
        {
            LazyThreadPooling.EnterTPViaQueueUserWorkItem();
        }

        private static void EnterTPViaAsynchronousDelegates()
        {
            LazyThreadPooling.EnterTPViaAsynchronousDelegates();
        }

        private static void EnterTPViaTaskParallelLibrary()
        {
            LazyThreadPooling.EnterTPViaTPL();
            LazyThreadPooling.EnterTPViaTPLWithResult();
        }

        private static void EnterTPViaBackgroundWorker()
        {
            LazyThreadPooling.EnterTPViaBackgroundWorker();
        }

        #endregion Thread pooling methods

        #region Exception handler methods

        private static void ExecuteWrongThreadHandlingPrior()
        {
            LazyExceptionHandler.ExecuteWrongThreadHandlingPrior();
        }

        private static void ExecuteThreadHandlingPrior()
        {
            LazyExceptionHandler.ExecuteThreadHandlingPrior();
        }

        private static void ExecuteCallerHandlingPrior()
        {
            LazyExceptionHandler.ExecuteCallerHandlingPrior();
        }

        private static void ExecuteThreadHandlingAbove()
        {
            LazyExceptionHandler.ExecuteThreadHandlingAbove();
        }

        private static void ExecuteCallerHandlingAbove()
        {
            LazyExceptionHandler.ExecuteCallerHandlingAbove();
        }

        #endregion Exception handler methods

        #region TaskCompletionSource methods

        private static void ExecuteDelayWithTCS()
        {
            TaskCompletionSourceHandler tcsHandler = new TaskCompletionSourceHandler();
            tcsHandler.ExecuteDelayWithTCS();
        }

        private static void ExecuteSdkMockWithTCS()
        {
            SdkProxy sdkProxy = new SdkProxy();

            // If we want to wait for each order to complete before starting the next one we should use await keyword
            sdkProxy.SubmitOrderAsync(10);
            sdkProxy.SubmitOrderAsync(20);
            sdkProxy.SubmitOrderAsync(5);
            sdkProxy.SubmitOrderAsync(15);
            sdkProxy.SubmitOrderAsync(4);
        }
        
        #endregion TaskCompletionSource methods

        #region Asynchronous patterns methods

        private static void DemonstrateAsynchronousProgrammingModelPattern()
        {
            string path = $"{Directory.GetCurrentDirectory()}\\..\\..\\Resources\\demo1.txt";
            FileInfo fileInfo = new FileInfo(path);
            
            if (!fileInfo.Exists)
            {
                Console.WriteLine("Specified file doesn't exist. Operation is canceled.");
                return;
            }

            APM apm = new APM();
            apm.ReadFileAsync(fileInfo);
        }

        private static void DemonstrateEventBasedAsynchronousPattern()
        {
            EAP eap = new EAP();
            eap.WorkDoneHandler += ProcessEAPResult;
            eap.DoWorkAsync();

            Console.WriteLine("Demonstration of Event-based Asynchronous Pattern is started...");
        }

        private static void ProcessEAPResult(EAPCompletedEventArgs args)
        {
            Console.WriteLine($"Demonstration of Event-based Asynchronous Pattern is done with result: {args.Result}");
        }

        private static void DemonstrateTaskBasedAsynchronousPattern()
        {
            TAP tap = new TAP();
            tap.ReadFilesSimultaneously();
        }

        #endregion Asynchronous patterns methods

        #region Attention for problems methods

        private static void DemonstrateCapturedVariablesProblem()
        {
            AttentionHandler handler = new AttentionHandler();
            handler.DemonstrateCapturedVariablesProblem();
        }

        #endregion Attention for problems methods

        #region Concurrent collections methods

        private static void CompareConcurentDictionaryWithLock()
        {
            ConcurrentColectionsHandler handler = new ConcurrentColectionsHandler();
            handler.CompareConcurentDictionaryWithLock();
        }

        #endregion Concurrent collections methods
    }
}
