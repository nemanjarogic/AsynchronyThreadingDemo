using System;
using System.Threading.Tasks;

namespace AsynchronyThreadingDemo.AsynchronousPatterns
{
    /// <summary>
    /// Event-based Asynchronous Pattern (EAP) is the event-based legacy model for providing asynchronous behavior. 
    /// It requires a method that has the Async suffix and one or more events, event handler delegate types, and EventArg-derived types. 
    /// BackgroundWorker (see ThreadPooling class in Demo directory) and WebClient are good examples of this pattern.
    /// It's no longer recommended for new development. 
    /// </summary>
    public class EAP
    {
        public delegate void EventHandler(EAPCompletedEventArgs args);
        public event EventHandler WorkDoneHandler;

        public async void DoWorkAsync()
        {
            Task workTask = SimulateWork();

            Console.WriteLine("DoWorkAsync continued execution...");

            await workTask;
            Console.WriteLine("DoWorkAsync completed work.");
        }

        /// <summary>
        /// You should prefer async Task to async void. Async Task methods enable easier error-handling, composability and testability. 
        /// The exception to this guideline is asynchronous event handlers, which must return void
        /// For more details see https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
        /// </summary>
        private async Task SimulateWork()
        {
            await Task.Delay(3000);
            WorkDoneHandler?.Invoke(new EAPCompletedEventArgs("Work is successfully done.", null, false, null));
        }
    }
}
