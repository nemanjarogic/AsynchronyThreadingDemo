using System;
using System.IO;
using System.Text;

namespace AsynchronyThreadingDemo.AsynchronousPatterns
{
    /// <summary>
    /// APM represent Asynchronous Programming Model Pattern
    /// An asynchronous operation that uses the IAsyncResult design pattern is implemented as two methods named BeginOperationName and EndOperationName that begin and end the asynchronous operation.
    /// After calling BeginOperationName, an application can continue executing instructions on the calling thread while the asynchronous operation takes place on a different thread.
    /// If the asynchronous operation represented by the IAsyncResult object has not completed when EndOperationName is called, EndOperationName blocks the calling thread until the asynchronous operation is complete.
    /// This pattern is no longer recommended for new development.
    /// </summary>
    public class APM
    {
        /// <summary>
        /// FileStream class provides the BeginRead and EndRead methods to asynchronously read bytes from a file. 
        /// </summary>
        public void ReadFileAsync(FileInfo file)
        {
            byte[] buffer = new byte[50];
            int readBytes = 0;

            Console.WriteLine($"Start asynchronously reading file {file.Name} from location {file.FullName}");

            using (FileStream stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, 1024, FileOptions.Asynchronous))
            {
                IAsyncResult result = stream.BeginRead(buffer, 0, buffer.Length, null, null);

                // do some work here while you wait
                Console.WriteLine($"File {file.Name} is started reading. Execution of method is continued...");

                //Calling EndRead will block until the Async work is complete
                readBytes = stream.EndRead(result);
            }

            Console.WriteLine($"Read {readBytes} bytes for specified file {file.Name}");
            Console.WriteLine($"Read content: {Encoding.UTF8.GetString(buffer, 0, readBytes)}");
        }
    }
}
