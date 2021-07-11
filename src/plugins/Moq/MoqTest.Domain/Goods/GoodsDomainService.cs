using System;
using System.Linq;
using MoqTest.Domain.Prop;

namespace MoqTest.Domain.Goods
{
    public class GoodsDomainService : IGoodsDomainService
    {
        private readonly IIdGenerator _idGenerator;

        public GoodsDomainService(IIdGenerator idGenerator)
        {
            _idGenerator = idGenerator;
        }

        protected virtual string CodeGenerator()
        {
            return $"{DateTime.Now.ToString("s")}";
        }

        public Goods CreateGoods(string name)
        {
            return new(_idGenerator.Create(), name, CodeGenerator());
        }

        public Goods AddProp(Goods goods, PropName name, PropValue value)
        {
            var goodsProp = new GoodsProp(_idGenerator.Create(), goods, name, value);

            goods.GoodsProps.Add(goodsProp);

            return goods;
        }

        public void DeleteProp(Goods goods, PropName name, PropValue value)
        {
            var goodsProp = goods.GoodsProps.First(o => o.PropNameId == name.Id && o.PropValueId == value.Id);

            goods.GoodsProps.Remove(goodsProp);
        }
    }
}