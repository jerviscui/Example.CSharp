using MoqTest.Domain.Prop;

namespace MoqTest.Domain.Goods
{
    public interface IGoodsDomainService
    {
        public Goods CreateGoods(string name);

        public Goods AddProp(Goods goods, PropName name, PropValue value);

        public void DeleteProp(Goods goods, PropName name, PropValue value);
    }
}
