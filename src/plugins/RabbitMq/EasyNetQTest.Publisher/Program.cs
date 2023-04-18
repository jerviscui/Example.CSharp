using EasyNetQTest.Publisher;

var simple = new SimplePublisher();
//simple.PublishTest();
simple.PublishThrowExceptionTest();

//await simple.PublishAsyncTest();

//simple.CustomNameTest();
//simple.PublishWithTopicTest();

//CustomConventionsPublisher.PublishTest();

//await PubConfirmPublisher.PublisherConfirmsTest();

Console.ReadLine();
