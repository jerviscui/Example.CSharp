using System;
using System.Threading;

namespace MemoryModelTest
{
    /// <summary>
    /// Memory Operation Reordering Tests
    /// </summary>
    public class MemoryReorderingTests
    {
        private static Thread w1 = new(o => ((ITester)o).Print());

        private static Thread w2 = new(o => ((ITester)o).Init());

        /// <summary>
        /// 当发生内存重排会输出 0
        /// </summary>
        public static void NonVolatile_Test()
        {
            int count = 1_000_000;
            while (count-- > 0)
            {
                var data = new DataInit();

                new Thread(() => { data.Print(); }).Start();
                new Thread(() => { data.Init(); }).Start();
            }
        }

        /// <summary>
        /// 当发生内存重排会输出 0
        /// </summary>
        public static void Volatile_data_Error_Test()
        {
            int count = 1_000_000;
            while (count-- > 0)
            {
                var data = new DataInit2();

                new Thread(() => { data.Print(); }).Start();
                new Thread(() => { data.Init(); }).Start();
            }
        }

        /// <summary>
        /// 当发生内存重排会输出 0
        /// </summary>
        public static void Volatile_initialized_Success_Test()
        {
            int count = 1_000_000;
            while (count-- > 0)
            {
                var data = new DataInit3();

                new Thread(() => { data.Print(); }).Start();
                new Thread(() => { data.Init(); }).Start();
            }
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
                    Console.WriteLine(_data); // Read 2 当发生内存重排会输出 0
                }
                else
                {
                    Console.WriteLine($"Not initialized {_data}");
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
                    Console.WriteLine(_data); // Read 2 当发生内存重排会输出 0
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
                    Console.WriteLine(_data); // Read 2 当发生内存重排会输出 0
                }
                else
                {
                    Console.WriteLine("Not initialized");
                }
            }
        }
    }
}
