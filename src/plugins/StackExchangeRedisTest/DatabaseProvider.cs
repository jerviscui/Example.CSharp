using StackExchange.Redis;

namespace StackExchangeRedisTest;

public static class DatabaseProvider
{

    #region Constants & Statics

    static DatabaseProvider()
    {
        Logger = new StringWriter();
        Connection = ConnectionMultiplexer.Connect("localhost:6379,DefaultDatabase=5,allowAdmin=true", Logger);
    }

    /// <summary>
    /// Gets the connection.
    /// </summary>
    public static ConnectionMultiplexer Connection { get; private set; }

    /// <summary>
    /// Gets the logger.
    /// </summary>
    public static StringWriter Logger { get; }

    public static void Dispose()
    {
        Connection.Dispose();
        Logger.Dispose();
    }

    /// <summary>
    /// Gets the database.
    /// </summary>
    public static IDatabase GetDatabase()
    {
        return Connection.GetDatabase();
    }

    /// <summary>
    /// Gets the subscriber.
    /// </summary>
    public static ISubscriber GetSubscriber()
    {
        return Connection.GetSubscriber();
    }

    public static void Start()
    {
        //clear
        //GetDatabase().Execute("flushdb");
    }

    #endregion

}
