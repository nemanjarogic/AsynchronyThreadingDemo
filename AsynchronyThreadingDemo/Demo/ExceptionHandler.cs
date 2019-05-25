using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronyThreadingDemo.Demo
{
    public class ExceptionHandler
    {
        #region Prior .NET Framework 4 - handling methods

        /// <summary>
        /// The try/catch statement in this example is ineffective and will cause crash of the whole application
        /// Be aware that each thread has an independent execution path.
        /// </summary>
        public void ExecuteWrongThreadHandlingPrior()
        {
            try
            {
                Console.WriteLine("ExecuteWrongHandlingPrior starting new thread...");
                Thread thread = new Thread(SimulateWork);
                thread.Start();
            }
            catch(Exception ex)
            {
                //We'll never get here!
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        /// <summary>
        /// Solution is to move exception handling to newly created thread
        /// </summary>
        public void ExecuteThreadHandlingPrior()
        {
            Console.WriteLine("ExecuteThreadHandlingPrior starting new thread...");
            Thread thread = new Thread(SimulateWorkWithExceptionHandling);
            thread.Start();
        }

        /// <summary>
        /// If we really need information about exception on caller's thread we can get that information through parameters
        /// </summary>
        public void ExecuteCallerHandlingPrior()
        {
            Console.WriteLine("ExecuteCallerHandlingPrior starting new thread...");

            Exception exception = null;
            Thread thread = new Thread(() => SimulateWorkWithPassingException(out exception));

            thread.Start();
            thread.Join();

            if(exception != null)
            {
                Console.WriteLine($"Caller caught exception: {exception.Message}");
            }
        }

        private void SimulateWork()
        {
            throw new InvalidOperationException();
        }

        private void SimulateWorkWithExceptionHandling()
        {
            try
            {
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private void SimulateWorkWithPassingException(out Exception exception)
        {
            exception = null;

            try
            {
                throw new InvalidOperationException();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        #endregion Prior .NET Framework 4 - handling methods

        #region .NET Framework 4 - handling methods

        public void ExecuteThreadHandlingAbove()
        {
            Task<int> task = new Task<int>(SimulateCalculationWork);
            task.ContinueWith(ExamineTaskExceptions, TaskContinuationOptions.OnlyOnFaulted);
            task.Start();
        }

        public void ExecuteCallerHandlingAbove()
        {
            Task<int> task = new Task<int>(SimulateCalculationWork);
            task.Start();

            try
            {
                //If task throws an unhandled exception, that exception is automatically re-thrown to whoever call Wait() or accesses the Result property of a Task
                task.Wait();
            }
            catch (AggregateException ex)
            {
                //The CLR wraps the exception in an AggregateException in order to play well with parallel programming scenarios
                if(ex.InnerException is NullReferenceException)
                {
                    Console.WriteLine($"Caller caught null reference exception: {ex.Message}");
                }
            }
        }

        private int SimulateCalculationWork()
        {
            throw new NullReferenceException();
        }

        private void ExamineTaskExceptions(Task<int> task)
        {
            AggregateException exception = task.Exception;
            Console.WriteLine($"Exception: {exception}");

        }

        #endregion .NET Framework 4 - handling methods
    }
}
