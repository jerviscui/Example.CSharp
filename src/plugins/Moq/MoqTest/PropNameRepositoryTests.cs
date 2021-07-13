using Moq;
using Moq.Protected;
using MoqTest.Domain.Prop;
using Shouldly;
using Xunit;

namespace MoqTest
{
    public class PropNameRepositoryTests
    {
        [Fact]
        public void IPropNameRepository_MoqPublicMethod_Test()
        {
            //moq interface public method
            var mock = new Mock<IPropNameRepository>();
            mock.Setup(repository => repository.DeletePropName(It.IsAny<long>())).Returns(true);

            var result = mock.Object.DeletePropName(1);

            result.ShouldBe(true);
        }

        [Fact]
        public void PropNameRepository_MoqPublicMethod_Test()
        {
            //moq class public method
            var mock = new Mock<PropNameRepository>();
            mock.Setup(repository => repository.DeletePropName(It.IsAny<long>())).Returns(true);

            var result = mock.Object.DeletePropName(1);

            result.ShouldBe(true);
        }

        [Fact]
        public void IPropNameRepository_MoqPrivateMethod_Test()
        {
            //moq interface protected virtual method
            var mock = new Mock<IPropNameRepository>();
            mock.CallBase = true;//
            mock.Protected().Setup<bool>("PrivateMethodForTest", ItExpr.IsAny<long>()).Returns(true);
            
            var result = mock.Object.Test();

            result.ShouldBe(true);
        }

        [Fact]
        public void PropNameRepository_MoqPrivateMethod_Test()
        {
            //moq class protected virtual method
            var mock = new Mock<PropNameRepository>();
            mock.Protected().Setup<bool>("PrivateMethodForTest", ItExpr.IsAny<long>()).Returns(true);
            
            var result = mock.Object.Test();

            mock.Protected().Verify<bool>("PrivateMethodForTest", Times.Once(), ItExpr.IsAny<long>());
            result.ShouldBe(true);
        }
    }
}
