namespace CapTest.Shared;

public class OrderCreatedEventData
{

    #region Constants & Statics

    public const string Name = "Order.Created";

    #endregion

    public OrderCreatedEventData(string number)
    {
        Number = number;
    }

    #region Properties

    public string Number { get; set; }

    #endregion

}
