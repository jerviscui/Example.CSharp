using System.IO;
using StackExchange.Redis;

namespace StackExchangeRedisTest
{
    internal class DatabaseProvider
    {
        static DatabaseProvider()
        {
            Logger = new StringWriter();
            Connection =
                Connection =
                    ConnectionMultiplexer.Connect("10.99.59.47:7000,10.99.59.47:7000,DefaultDatabase=5,allowAdmin=true",
                        Logger);
            //Connection = ConnectionMultiplexer.Connect("localhost:6379,DefaultDatabase=5", Logger);
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        public static ConnectionMultiplexer Connection { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        public static StringWriter Logger { get; }

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
            GetDatabase().Execute("flushdb");
        }

        public static void Dispose()
        {
            Connection.Dispose();
            Logger.Dispose();
        }
    }
}
