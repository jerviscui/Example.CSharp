using System.Diagnostics;

namespace DiagnosticTest;

internal class TestTodoObserver : IObserver<DiagnosticListener>
{
    /// <inheritdoc />
    public void OnCompleted()
    {
        Console.WriteLine();
    }

    /// <inheritdoc />
    public void OnError(Exception error)
    {
        Console.WriteLine();
    }

    /// <inheritdoc />
    public void OnNext(DiagnosticListener value)
    {
        Console.WriteLine($"{value.Name}");

        //subscribe
        if (value.Name == "Test")
        {
            value.Subscribe(new DiagnosticObserver(), s => s.Equals("todo"));
        }
    }
}
