using System;
using System.Threading;

namespace ThreadingTest;

public static class InterruptTest
{
    public static void Sleeping_Interrupt_Test()
    {
        // Interrupt a sleeping thread.
        var sleepingThread = new Thread(SleepIndefinitely);
        sleepingThread.Name = "Sleeping";
        sleepingThread.Start();
        Thread.Sleep(2000);
        sleepingThread.Interrupt();

        //sleepingThread = new Thread(SleepIndefinitely);
        //sleepingThread.Name = "Sleeping2";
        //sleepingThread.Start();
        //Thread.Sleep(2000);
        //sleepingThread.Abort(); // replace by  CancellationToken
    }

    private static void SleepIndefinitely()
    {
        Console.WriteLine("Thread '{0}' about to sleep indefinitely.",
            Thread.CurrentThread.Name);
        try
        {
            Thread.Sleep(Timeout.Infinite);
        }
        catch (ThreadInterruptedException)
        {
            Console.WriteLine("Thread '{0}' awoken.",
                Thread.CurrentThread.Name);
        }
        catch (ThreadAbortException)
        {
            Console.WriteLine("Thread '{0}' aborted.",
                Thread.CurrentThread.Name);
        }
        finally
        {
            Console.WriteLine("Thread '{0}' executing finally block.",
                Thread.CurrentThread.Name);
        }
        Console.WriteLine("Thread '{0} finishing normal execution.",
            Thread.CurrentThread.Name);
        Console.WriteLine();
    }
}
