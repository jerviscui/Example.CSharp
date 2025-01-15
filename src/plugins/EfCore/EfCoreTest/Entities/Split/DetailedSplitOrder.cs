namespace EfCoreTest;

public class DetailedSplitOrder : Entity
{

    #region Properties

    public string BillingAddress { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public OrderStatus? Status { get; set; }

    #endregion

}
