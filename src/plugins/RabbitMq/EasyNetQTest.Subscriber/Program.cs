using EasyNetQTest.Subscriber;

var simple = new SimpleSubscriber();
//var result = simple.SubscribeTest();
//result.Dispose();

//result = await simple.SubscribeAsyncTest();
//result.Dispose();

//_ = simple.CustomNameTest();
//_ = simple.SubscribeWithTopicTest();

//simple.SubscribeThrowExceptionTest();

//CustomConventionsSubscriber.SubscribeTest();

//SimpleConsumerErrorStrategySubscriber.SubscribeTest();
SimpleConsumerErrorStrategySubscriber.SubscribeThrowExceptionTest();

//var containerSubscriber = new CustomContainerSubscriber();
//await containerSubscriber.SubscribeTest();

//var auto = new AutoConsumerSubscriber();
//await auto.AutoSubscriberTest();

Console.ReadLine();
