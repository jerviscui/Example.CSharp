using System;
using Moq;
using Moq.Protected;
using MoqTest.Domain.Prop;
using Shouldly;
using Xunit;

namespace MoqTest
{
    public class PropValueTests
    {
        [Fact]
        public void ctor_MockPropName_Test()
        {
            //arrange
            var mock = new Mock<PropName>();

            //act
            var propValue = new PropValue(1, "prop value", mock.Object);

            //assert
            propValue.Id.ShouldBe(1);
        }

        [Fact]
        public void PropValue_MockPublicProperty_Test()
        {
            //moq public virtual prop
            var mock = new Mock<PropValue>();
            //mock.Setup(value => value.Value).Returns("value");

            mock.SetupSet(value => value.Value = It.IsAny<string>());//.Throws();

            mock.Object.Value = "aa";

            //assert
            mock.VerifySet(value => value.Value = It.IsAny<string>(), Times.Once);
            mock.VerifySet(value => value.Value = "aa", Times.Once);


            mock.Object.Value.ShouldBe("value");
        }

        [Fact]
        public void PropValue_MockPrivateProperty_Test()
        {
            //moq protected virtual prop
            var mock = new Mock<PropValue>();
            //mock.Protected().Setup<string>("PrivatePropForTest", ItExpr.IsAny<string>()).Returns("test");
            //mock.SetupAllProperties();
            mock.CallBase = true;

            mock.Protected().SetupSet<string>("PrivatePropForTest", "aa");

            mock.Object.SetTest("aa");
            var result = mock.Object.GetTest();
            result.ShouldBe("aa");
            mock.Object.TestFiled.ShouldBe("aa");
            mock.Protected().VerifySet<string>("PrivatePropForTest", Times.Once(), It.Is<string>(s => s == null));


            mock.Protected().SetupGet<string>("PrivatePropForTest").Returns("test");

            result = mock.Object.GetTest();
            //assert get
            result.ShouldBe("test");
        }

        [Fact]
        public void PropValue_MockProtectedPropName_Test()
        {
            //arrange
            //var mock = new Mock<PropValue>();
            //mock.Setup(value => value.PropNameId).Returns(1);//throw NotSupportedException

            //moq protected property
            var propValue = Mock.Of<PropValue>(value => value.Id == 1 &&
                                                        value.PropName == Mock.Of<PropName>(name => name.Name == "prop name"));

            //assert
            propValue.Id.ShouldBe(1);
            propValue.PropName.Name.ShouldBe("prop name");
        }

        [Fact]
        public void MockPropValue_VirtualGetter_Test()
        {
            //moq property
            var mock = new Mock<PropValue>();

            mock.SetupGet(value => value.Id).Returns(1);
            //mock.Setup(value => value.Id).Returns(1);

            //assert
            mock.Object.Id.ShouldBe(1);
        }

        [Fact]
        public void MockPropValue_VirtualGetter_Test2()
        {
            //moq property
            var mock = new Mock<PropValue>();

            mock.Protected().SetupSet<long>("PropNameId", 10);
            mock.Object.SetPropNameId(mock.Object);
            //mock.SetupProperty(o => o.PropNameId);

            //mock.VerifySet(value => value.PropNameId, Times.Once());

            mock.Object.PropNameId.ShouldBe(1);
        }

        [Fact]
        public void MockPropValueId_DefaultValue_Test2()
        {
            //moq property with default value
            var mock = new Mock<PropValue>();

            mock.SetupProperty(value => value.Id, 10);

            mock.Object.Id.ShouldBe(10);
        }
    }
}