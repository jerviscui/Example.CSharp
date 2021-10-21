namespace CapTest.Order.Service
{
    public class OrderCreatedEventData
    {
        public const string Name = "Order.Created";

        public OrderCreatedEventData(string number) => Number = number;

        public string Number { get; set; }
    }
}
