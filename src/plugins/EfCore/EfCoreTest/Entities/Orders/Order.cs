namespace EfCoreTest
{
    public class Order : Entity
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        protected Order()
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
        }

        /// <inheritdoc />
        public Order(long id, string buyer, string street, string city)
        {
            Id = id;
            Buyer = buyer;
            StreetAddress = new StreetAddress(street, city);
        }

        public string Buyer { get; protected set; }

        public StreetAddress StreetAddress { get; protected set; }
    }
}
