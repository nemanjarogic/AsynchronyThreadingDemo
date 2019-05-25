using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;

namespace AsynchronyThreadingDemo.Demo
{
    public class ConcurrentColectionsHandler
    {
        #region Fields

        private readonly int _numberOfIterations = 10000000;

        #endregion Fields

        #region Public methods


        public void CompareConcurentDictionaryWithLock()
        {
            long concurrentDictTime = ExecuteConcurrentDictonaryOperation();
            Console.WriteLine($"Concurrent dictionary finished after {concurrentDictTime} ms");

            long lockTime = ExecuteLockOperation();
            Console.WriteLine($"Lock finished after {lockTime} ms");
        }

        #endregion Public methods

        #region Private methods

        private long ExecuteConcurrentDictonaryOperation()
        {
            Console.WriteLine("Starting for loop with ConcurrentDictionary...");
            Stopwatch stopwach = Stopwatch.StartNew();

            ConcurrentDictionary<int, int> concurrentDict = new ConcurrentDictionary<int, int>();
            //int sum = 0;

            for (int index = 0; index < _numberOfIterations; index++)
            {
                concurrentDict[index] = 123;
                // Reading from a ConcurrentDictionary is fast because reads are lockfree
                //sum += concurrentDict[index];
            }

            stopwach.Stop();
            return stopwach.ElapsedMilliseconds;
        }

        private long ExecuteLockOperation()
        {
            Console.WriteLine("Starting for loop with lock...");
            Stopwatch stopwach = Stopwatch.StartNew();

            Dictionary<int, int> dict = new Dictionary<int, int>();
            //int sum = 0;

            for (int index = 0; index < _numberOfIterations; index++)
            {
                lock (dict)
                {
                    dict[index] = 123;
                    //sum += dict[index];
                }
            }

            stopwach.Stop();
            return stopwach.ElapsedMilliseconds;
        }

        #endregion Private methods
    }
}
