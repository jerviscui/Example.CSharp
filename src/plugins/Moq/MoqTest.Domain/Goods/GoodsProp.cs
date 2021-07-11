using MoqTest.Domain.Prop;

namespace MoqTest.Domain.Goods
{
    public class GoodsProp : Entity
    {
        public long GoodsId { get; protected set; }

        public Goods Goods { get; protected set; } = null!;

        public long PropNameId { get; protected set; }

        public PropName PropName { get; protected set; } = null!;

        public long PropValueId { get; protected set; }

        public PropValue PropValue { get; protected set; } = null!;

        protected GoodsProp()
        {

        }

        public GoodsProp(long id, Goods goods, PropName propName, PropValue propValue)
        {
            Id = id;
            GoodsId = goods.Id;
            Goods = goods;
            PropNameId = propName.Id;
            PropName = propName;
            PropValueId = propValue.Id;
            PropValue = propValue;
        }
    }
}
