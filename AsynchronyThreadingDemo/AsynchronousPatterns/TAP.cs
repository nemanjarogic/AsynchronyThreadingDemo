using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsynchronyThreadingDemo.AsynchronousPatterns
{
    /// <summary>
    /// Task-based Asynchronous Pattern (TAP), which uses a single method to represent the initiation and completion of an asynchronous operation. 
    /// The async and await keywords in C# add language support for TAP.
    /// It's the recommended approach to asynchronous programming in .NET. 
    /// </summary>
    public class TAP
    {
        public void ReadFilesSimultaneously()
        {
            List<FileInfo> files = GetFilesForReading();
            if(!files.Any())
            {
                return;
            }

            Task<int>[] readingTasks = new Task<int>[files.Count];
            for(int index = 0; index  < files.Count; index++)
            {
                FileInfo file = files[index];
                readingTasks[index] = ReadFileAsync(file);

                Console.WriteLine($"File read for {file.Name} has started...");

                //readingTasks[index].Wait();
                //Console.WriteLine($"Client got read {readingTasks[index].Result} bytes from file {file.Name}");
            }

            Console.WriteLine($"Reading of all files is started. Method execution is continued...");

            // Example of concurrent composition use
            Task.WhenAll(readingTasks).ContinueWith(task => Console.WriteLine("\n\nAll files have been read successfully."));
        }

        private List<FileInfo> GetFilesForReading()
        {
            List<FileInfo> files = new List<FileInfo>();

            foreach (string fileName in new string[] { "demo1.txt", "demo2.txt" })
            {
                string path = $"{Directory.GetCurrentDirectory()}\\..\\..\\Resources\\{fileName}";
                FileInfo fileInfo = new FileInfo(path);

                if (fileInfo.Exists)
                {
                    files.Add(fileInfo);
                }
            }

            return files;
        }

        private async Task<int> ReadFileAsync(FileInfo file)
        {
            int readBytes = 0;

            using (FileStream stream = File.OpenRead(file.FullName))
            {
                byte[] buffer = new byte[stream.Length];
                Task<int> readTask = stream.ReadAsync(buffer, 0, (int)stream.Length);

                readBytes = await readTask;

                if (readTask.IsCompleted)
                {
                    Console.WriteLine($"Read {readBytes} bytes successfully from file {file.Name}");
                    Console.WriteLine($"Read content: {Encoding.UTF8.GetString(buffer, 0, readBytes)}");
                }
            }

            return readBytes;
        }
    }
}
