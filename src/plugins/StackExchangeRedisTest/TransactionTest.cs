using System.Threading.Tasks;
using StackExchange.Redis;

namespace StackExchangeRedisTest
{
    internal sealed class TransactionTest
    {
        public static async Task Transaction_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            database.StringSet("version", 1);
            var tran = database.CreateTransaction(1);

            tran.AddCondition(Condition.StringEqual("version", 1));

            var nameTask = tran.StringSetAsync("data.name", "this is name");
            var typeTask = tran.StringSetAsync("data.type", "test");

            //var b = tran.Execute();
            var commited = await tran.ExecuteAsync(); //Exec 才会发送 Transaction 包含的命令

            if (commited)
            {
                var r1 = nameTask.Result;
                var r2 = typeTask.Result;
            }

            //产生命令
            //"SET" "version" "1"
            //await tran.ExecuteAsync()
            //"WATCH" "version"
            //"GET" "version"
            //"MULTI"
            //"SET" "data.name" "this is name"
            //"SET" "data.type" "test"
            //"EXEC"
        }

        public static async Task ChangeWatchValue_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            database.StringSet("version", 1);
            var tran = database.CreateTransaction();

            tran.AddCondition(Condition.StringEqual("version", 1));

            var nameTask = tran.StringSetAsync("data.name", "this is name");
            //await 会阻塞程序运行
            //var nameTask = await tran.StringSetAsync("data.name", "this is name");
            var typeTask = tran.StringSetAsync("data.type", "test");
            var incrementAsync = tran.StringIncrementAsync("version");

            var commited = await tran.ExecuteAsync();

            if (commited)
            {
                var r1 = nameTask.Result;
                var r2 = typeTask.Result;
            }

            //"SET" "version" "1"
            //await tran.ExecuteAsync()
            //"WATCH" "version"
            //"GET" "version"
            //"MULTI"
            //"SET" "data.name" "this is name"
            //"SET" "data.type" "test"
            //"INCR" "version"
            //"EXEC"
        }

        public static async Task MultiCondition_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            var tran = database.CreateTransaction();

            //条件需要同时满足才执行，
            tran.AddCondition(Condition.KeyNotExists("key"));
            tran.AddCondition(Condition.HashEqual("key", "version", 1));

            var nameTask = tran.StringSetAsync("data.name", "this is name");
            var typeTask = tran.StringSetAsync("data.type", "test");

            var commited = await tran.ExecuteAsync();
            //commited is false

            //"WATCH" "key"
            //"EXISTS" "key"
            //"WATCH" "key"
            //"HGET" "key" "version"
            //"UNWATCH"
        }
    }
}
