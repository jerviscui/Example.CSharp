using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MoqTest.Domain;
using MoqTest.Domain.Goods;
using Xunit;

namespace MoqAutofacTest
{
    /// <summary>
    /// 使用 Moq 替换默认注册服务，Scope 容器级别
    /// </summary>
    public class DefaultIdGeneratorTests : TestBase
    {
        protected override void PreInit(IServiceCollection services)
        {
            services.AddScoped<IGoodsDomainService, GoodsDomainService>();
        }

        [Fact]
        public void CreateGoods_ByDefaultIdGenerator_IdStartFromOne()
        {
            var service = ServiceProvider.GetRequiredService<IGoodsDomainService>();

            var goods1 = service.CreateGoods("t");
            var goods2 = service.CreateGoods("t");

            Assert.Equal(1, goods1.Id);
            Assert.Equal(2, goods2.Id);
        }

        [Fact]
        public void CreateGoods_ByMoqIdGenerator_IdAlwaysOne()
        {
            var mock = new Mock<IIdGenerator>();
            mock.Setup(o => o.Create()).Returns(1);

            //todo: how to replace ServiceProvider.CreateScope() registered service
            //using var serviceScope = ServiceProvider.CreateScope();
            //基于 Autofac 容器
            using var serviceScope = CreateScope(builder => builder.RegisterInstance(mock.Object));
            var service = serviceScope.Resolve<IGoodsDomainService>();

            var goods1 = service.CreateGoods("t");
            var goods2 = service.CreateGoods("t");

            Assert.Equal(1, goods1.Id);
            Assert.Equal(1, goods2.Id);
        }
    }
}