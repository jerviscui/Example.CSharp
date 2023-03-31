using EasyNetQ;

namespace EasyNetQTest.Publisher;

internal class PubConfirmPublisher
{
    public static async Task PublisherConfirmsTest()
    {
        var tasks = new Task<bool>[100];

        for (var i = 0; i < tasks.Length; i++)
        {
            tasks[i] = PublishAsync(i);
        }

        await Task.WhenAll(tasks);
    }

    private static async Task<bool> PublishAsync(int i)
    {
        try
        {
            await BusFactory.GetPubConfirmBus().PubSub
                .PublishAsync(new CustomNameMessage { Text = $"{i} : {DateTime.Now.ToShortTimeString()}" });

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return false;
    }
}
