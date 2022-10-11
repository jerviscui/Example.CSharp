namespace EfCoreTest;

public class DetailedSplitOrder : Entity
{
    public OrderStatus? Status { get; set; }

    public string BillingAddress { get; set; }

    public string ShippingAddress { get; set; }
}
