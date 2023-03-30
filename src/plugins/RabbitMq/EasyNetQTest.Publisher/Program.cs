using EasyNetQTest.Publisher;

var simple = new SimplePublisher();
//simple.PublishTest();
//await simple.PublishAsyncTest();

//simple.CustomNameTest();
//simple.PublishWithTopicTest();

CustomConventionsPublisher.PublishTest();

Console.ReadLine();
