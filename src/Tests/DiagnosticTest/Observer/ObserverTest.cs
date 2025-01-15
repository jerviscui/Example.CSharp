namespace DiagnosticTest;

internal sealed class ObserverTest
{
    public static void SimpleTest()
    {
        using var publisher = new Publisher();

        var sub1 = new Subscriber();
        sub1.Register(publisher);

        publisher.Publish(new Data { Name = $"created at {DateTime.Now}" });

        var sub2 = new Subscriber();
        sub2.Register(publisher);
    }
}
