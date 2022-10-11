using System.ComponentModel.DataAnnotations;

namespace EfCoreTest
{
    public class StreetAddress
    {
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        protected StreetAddress()
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
        }

        public StreetAddress(string street, string city)
        {
            Street = street;
            City = city;
        }

        [StringLength(200)]
        public string Street { get; protected set; }

        [StringLength(200)]
        public string City { get; protected set; }
    }
}
