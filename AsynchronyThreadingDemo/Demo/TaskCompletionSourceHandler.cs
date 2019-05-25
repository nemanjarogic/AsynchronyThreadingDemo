using System;
using System.Threading.Tasks;
using System.Timers;

namespace AsynchronyThreadingDemo.Demo
{
    public class TaskCompletionSourceHandler
    {
        public void ExecuteDelayWithTCS()
        {
            Console.WriteLine("Starting delay with 5000 miliseconds...");
            Delay(5000).GetAwaiter().OnCompleted(() => Console.WriteLine("Delay is done."));

            Console.WriteLine("Execution is continued...");
        }

        private Task Delay(double miliseconds)
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();

            Timer timer = new Timer(miliseconds) { AutoReset = false };
            timer.Elapsed += delegate 
            {
                timer.Dispose();
                tcs.SetResult(null);
            };

            timer.Start();

            return tcs.Task;
        }
    }
}
