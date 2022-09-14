namespace EfCoreTest;

public class SplitOrder : Entity
{
    public SplitOrder(long id)
    {
        Id = id;
    }

    public OrderStatus? Status { get; set; }

    public DetailedSplitOrder DetailedSplitOrder { get; set; }
}
