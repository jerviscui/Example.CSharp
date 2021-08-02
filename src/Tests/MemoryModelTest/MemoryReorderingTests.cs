using System;
using System.Threading.Tasks;

namespace MemoryModelTest
{
    /// <summary>
    /// Memory Operation Reordering Tests
    /// </summary>
    public class MemoryReorderingTests
    {
        /// <summary>
        /// 当发生内存重排会输出 0
        /// </summary>
        public static void NonVolatile_Test()
        {
            var array = new DataInit[50];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new DataInit();
            }

            Exec(array);
        }

        /// <summary>
        /// 当发生内存重排会输出 Not initialized
        /// </summary>
        public static void NonVolatile__Test()
        {
            var array = new DataInit2[50];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new DataInit2();
            }

            Exec(array);
        }

        /// <summary>
        /// 当发生内存重排会输出 Not initialized
        /// </summary>
        public static void NonVolatile___Test()
        {
            var array = new DataInit3[50];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new DataInit3();
            }

            Exec(array);
        }

        private static void Exec(ITester[] array)
        {
            var tasks = new Task[array.Length * 2];

            for (var i = 0; i < tasks.Length; i++)
            {
                var i1 = i;
                if (i % 2 == 0)
                {
                    tasks[i] = new Task(() =>
                    {
                        array[i1 / 2].Init();
                    });
                }
                else
                {
                    tasks[i] = new Task(() =>
                    {
                        array[i1 / 2].Print();
                    });
                }
            }

            Parallel.ForEach(tasks,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 },
                task => task.Start());

            Task.WaitAll(tasks);
        }

        private interface ITester
        {
            public void Init();

            public void Print();
        }

        /// <summary>
        /// non-volatile
        /// </summary>
        private class DataInit : ITester
        {
            private int _data; //non-volatile

            private bool _initialized; //non-volatile

            public void Init()
            {
                _data = 42;          // Write 1
                _initialized = true; // Write 2
            }

            public void Print()
            {
                if (_initialized) // Read 1
                {
                    Console.WriteLine(_data); // Read 2
                }
                else
                {
                    Console.WriteLine("Not initialized");
                }
            }
        }

        /// <summary>
        /// volatile _data
        /// still memory reordering
        /// </summary>
        private class DataInit2 : ITester
        {
            private volatile int _data;

            private bool _initialized; //non-volatile

            public void Init()
            {
                _data = 42;          // Write 1
                _initialized = true; // Write 2
            }

            public void Print()
            {
                if (_initialized) // Read 1
                {
                    Console.WriteLine(_data); // Read 2
                }
                else
                {
                    Console.WriteLine("Not initialized");
                }
            }
        }

        /// <summary>
        /// volatile _initialized
        /// no memory reordering
        /// </summary>
        private class DataInit3 : ITester
        {
            private int _data; //non-volatile

            private volatile bool _initialized;

            public void Init()
            {
                _data = 42;          // Write 1
                _initialized = true; // Write 2
            }

            public void Print()
            {
                if (_initialized) // Read 1
                {
                    Console.WriteLine(_data); // Read 2
                }
                else
                {
                    Console.WriteLine("Not initialized");
                }
            }
        }
    }
}
