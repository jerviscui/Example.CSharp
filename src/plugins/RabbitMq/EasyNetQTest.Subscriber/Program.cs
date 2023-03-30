using EasyNetQTest.Subscriber;

var simple = new SimpleSubscriber();
//var result = simple.SubscribeTest();
//result.Dispose();

//result = await simple.SubscribeAsyncTest();
//result.Dispose();

//_ = simple.CustomNameTest();
//_ = simple.SubscribeWithTopicTest();

CustomConventionsSubscriber.SubscribeTest();

Console.ReadLine();
