using Autofac;
using Autofac.Extras.Moq;
using Moq;
using Shouldly;
using Xunit;

namespace MoqAutofacTest
{
    /// <summary>
    /// 使用 Autofac.Extras.Moq 
    /// </summary>
    public class AutofacExtrasMoqTests
    {
        //todo: implement

        public class ServiceClass
        {
            private readonly IDependency _dependency;

            public ServiceClass(IDependency dependency)
            {
                _dependency = dependency;
            }

            public virtual int Do()
            {
                return _dependency.Do();
            }
        }

        public interface IDependency
        {
            public int Do();
        }

        [Fact]
        public void AutoMock_DefaultBehavior_Test()
        {
            using var autoMock = AutoMock.GetLoose();
            var test = autoMock.Create<ServiceClass>();

            test.Do().ShouldBe(0);
        }

        [Fact]
        public void AutoMock_CustomBehavior_Test()
        {
            using var autoMock = AutoMock.GetStrict();
            autoMock.Mock<IDependency>().Setup(o => o.Do()).Returns(10);
            var test = autoMock.Create<ServiceClass>();

            test.Do().ShouldBe(10);
        }

        private class Dependency : IDependency
        {
            public int Do()
            {
                return 2;
            }
        }

        [Fact]
        public void AutoMock_ConfigureContainerBuilder_Test()
        {
            var dependency = new Dependency();

            using var autoMock = AutoMock.GetLoose(builder => builder.RegisterInstance(dependency).As<IDependency>());
            var test = autoMock.Create<ServiceClass>();

            test.Do().ShouldBe(2);
        }

        [Fact]
        public void AutoMock_UseMock_Test()
        {
            var mock = new Mock<IDependency>();
            mock.Setup(o => o.Do()).Returns(10);

            using var autoMock = AutoMock.GetLoose(builder => builder.RegisterMock(mock));
            var test = autoMock.Create<ServiceClass>();

            test.Do().ShouldBe(10);
        }
    }
}
