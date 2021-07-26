using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using MoqTest.Domain;
using MoqTest.Domain.Goods;
using Xunit;

namespace MoqAutofacTest
{
    /// <summary>
    /// 使用 Moq 替换默认注册服务，根容器级别
    /// </summary>
    public class MoqIdGeneratorTests : TestBase
    {
        protected override void PreInit(IServiceCollection services)
        {
            services.AddScoped<IGoodsDomainService, GoodsDomainService>();

            var mock = new Mock<IIdGenerator>();
            mock.Setup(o => o.Create()).Returns(1);
            services.Replace(ServiceDescriptor.Singleton(mock.Object));
        }

        [Fact]
        public void CreateGoods_ByMoqIdGenerator_IdAlwaysOne()
        {
            var service = ServiceProvider.GetRequiredService<IGoodsDomainService>();

            var goods1 = service.CreateGoods("t");
            var goods2 = service.CreateGoods("t");

            Assert.Equal(1, goods1.Id);
            Assert.Equal(1, goods2.Id);
        }
    }
}
