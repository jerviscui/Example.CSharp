using Mapster;
using MapsterTest.Domains;
using MapsterTest.DomainsDto;

namespace MapsterTest
{
    public class MyRegister : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Student, StudentDto2>().MaxDepth(3);
        }
    }
}
