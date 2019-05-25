using System;
using System.Threading.Tasks;

namespace AsynchronyThreadingDemo.Attention
{
    /// <summary>
    /// AttentionHandler is used to demonstrate interesting problems in multi-threading use case scenarious
    /// </summary>
    public class AttentionHandler
    {
        /// <summary>
        /// The problem is that the variable refers to the same memory location throughout the loop’s lifetime
        /// Therefore, each thread calls Console.Write on a variable whose value may change as it is running
        /// The solution is to use a temporary variable
        /// </summary>
        public void DemonstrateCapturedVariablesProblem()
        {
            Console.WriteLine("Captured variables problem: ");
            for(int index = 0; index < 10; index++)
            {
                Task.Run(() => Console.Write(index + ", "));
            }

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();

            Console.WriteLine("Solution with captured variables is to use temporary variable: ");
            for (int index = 0; index < 10; index++)
            {
                int temp = index;
                Task.Run(() => Console.Write(temp + ", "));
            }

            Console.WriteLine("Press enter to continue...");
            Console.ReadLine();
        }
    }
}
