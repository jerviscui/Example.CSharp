using System.Collections.Generic;

namespace MoqTest.Domain.Goods
{
    public class Goods : Entity
    {
#pragma warning disable 8618
        protected Goods()
#pragma warning restore 8618
        {
        }

        internal Goods(long id, string name, string code)
        {
            Id = id;
            Name = name;
            Code = code;
        }

        public string Name { get; protected set; }

        public string Code { get; protected set; }

        public List<GoodsProp> GoodsProps { get; protected set; } = new();
    }
}
