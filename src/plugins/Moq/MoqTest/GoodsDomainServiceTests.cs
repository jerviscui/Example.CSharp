using System;
using Moq;
using Moq.Protected;
using MoqTest.Domain;
using MoqTest.Domain.Goods;
using Shouldly;
using Xunit;

namespace MoqTest
{
    public class GoodsDomainServiceTests
    {
        [Fact]
        public void CreateGoods_UseDefaultIdGenerator_Test()
        {
            var goodsDomainService = new GoodsDomainService(new DefaultIdGenerator());

            var goods = goodsDomainService.CreateGoods("a goods");

            goods.Id.ShouldBe(1);
        }

        [Fact]
        public void CreateGoods_UseMoqIdGenerator_Test()
        {
            var mock = new Mock<IIdGenerator>();
            mock.Setup(generator => generator.Create()).Returns(10);
            var goodsDomainService = new GoodsDomainService(mock.Object);

            var goods = goodsDomainService.CreateGoods("a goods");

            goods.Id.ShouldBe(10);
        }

        [Fact]
        public void CreateGoods_UseMoqIdGenerator_Throw()
        {
            var mock = new Mock<IIdGenerator>();
            mock.Setup(generator => generator.Create()).Throws(new NotImplementedException());
            var goodsDomainService = new GoodsDomainService(mock.Object);

            Should.Throw<NotImplementedException>(() => goodsDomainService.CreateGoods("a goods"));
        }

        [Fact]
        public void CreateGoods_UseMoqCodeGenerator_Test()
        {
            var mock = new Mock<GoodsDomainService>(() => new GoodsDomainService(new DefaultIdGenerator()));
            mock.Protected().Setup<string>("CodeGenerator").Returns("moq code");

            var goods = mock.Object.CreateGoods("a goods");

            goods.Code.ShouldBe("moq code");
        }
    }
}