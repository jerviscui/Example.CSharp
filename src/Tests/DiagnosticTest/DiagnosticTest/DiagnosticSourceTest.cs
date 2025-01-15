using System.Diagnostics;
using Microsoft.Extensions.DiagnosticAdapter;

namespace DiagnosticTest;

internal sealed class DiagnosticSourceTest
{
    public static void DiagnosticListener_Subscribe_Test()
    {
        DiagnosticSource diagnostic = new DiagnosticListener("TestDiagnostic");

        var disposable = ((IObservable<KeyValuePair<string, object?>>)diagnostic).Subscribe(new DiagnosticObserver());

        var t = Task.Run(() =>
        {
            int count = 0;
            while (count++ < 5)
            {
                var isEnabled = diagnostic.IsEnabled("todo");
                Console.WriteLine($"#{count}with{Environment.CurrentManagedThreadId}: todo enable? {isEnabled}");
                if (isEnabled)
                {
                    Console.WriteLine($"Write with{Environment.CurrentManagedThreadId}");
                    diagnostic.Write("todo", new { Message = "this is todo log." });
                }

                Thread.Sleep(1000);
            }
        }).ContinueWith(_ =>
        {
            //disposable.Dispose(); //不会调用 OnCompleted
            ((IDisposable)diagnostic).Dispose();
        });

        t.GetAwaiter().GetResult();
    }

    public static void AllListeners_Subscribe_Test()
    {
        var diagnostic = new DiagnosticListener("Test");
        var diagnostic2 = new DiagnosticListener("TestTwice");

        DiagnosticListener.AllListeners.Subscribe(new TestTodoObserver());

        var t = Task.Run(() =>
        {
            int count = 0;
            while (count++ < 5)
            {
                var isEnabled = diagnostic.IsEnabled("todo");
                Console.WriteLine($"#{count}@{diagnostic.Name}: todo enable? {isEnabled}");
                if (isEnabled)
                {
                    diagnostic.Write("todo", new { Message = "this is todo log." });
                }
                else
                {
                    diagnostic.Write("simple", new { Message = "this is a simple." });
                }

                Thread.Sleep(1000);
            }
        });

        var t2 = Task.Run(() =>
        {
            int count = 0;
            while (count++ < 5)
            {
                var isEnabled = diagnostic2.IsEnabled("todo");
                Console.WriteLine($"#{count}@{diagnostic2.Name}: todo enable? {isEnabled}");
                if (isEnabled)
                {
                    diagnostic2.Write("todo", new { Message = "this is todo log." });
                }

                Thread.Sleep(1000);
            }
        });

        Task.WaitAll(t, t2);
    }

    public static void DiagnosticAdapter_Test()
    {
        var diagnostic = new DiagnosticListener("Test.Adapter");

        diagnostic.SubscribeWithAdapter(new MyAdapter());

        var t = Task.Run(() =>
        {
            int count = 0;
            while (count++ < 5)
            {
                var isEnabled = diagnostic.IsEnabled("todo");
                Console.WriteLine($"#{count}@{diagnostic.Name}: todo enable? {isEnabled}");
                if (isEnabled)
                {
                    diagnostic.Write("todo", new { data = new MyAdapter { Name = DateTime.Now.ToString("O") } });
                }

                Thread.Sleep(1000);
            }
        });

        t.GetAwaiter().GetResult();
    }
}

public class MyAdapter
{
    public string Name { get; set; } = null!;

    [DiagnosticName("todo")]
    public static void M(MyAdapter data)
    {
        Console.WriteLine(data.Name);
    }
}
