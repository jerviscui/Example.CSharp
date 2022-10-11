using System.Diagnostics.CodeAnalysis;
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
        public void Ctor_MockPropName_Test()
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
            mock.Setup(value => value.Value).Returns("value");

            mock.Object.Value = "aa";

            mock.VerifySet(value => value.Value = It.IsAny<string>(), Times.Once);
            mock.VerifySet(value => value.Value = "aa", Times.Once);
            mock.Object.Value.ShouldBe("value");
        }

        [Fact]
        public void PropValue_MockPublicProperty_GetterOrSetter()
        {
            //moq public virtual prop
            var mock = new Mock<PropValue>();

            mock.SetupGet(value => value.Value).Returns("value");
            mock.SetupSet(value => value.Value = "aa").CallBase();

            mock.Object.Value = "aa";
            mock.Object.Value = "aaa";

            //assert
            mock.VerifySet(value => value.Value = "aa", Times.Once);
            mock.VerifySet(value => value.Value = It.IsAny<string>(), Times.Exactly(2));
            mock.Object.Value.ShouldBe("value");
        }

        [Fact]
        public void PropValue_MockPublicProperty_SetupAllProperties()
        {
            var mock = new Mock<PropValue>();

            //init properties
            mock.SetupAllProperties();
            mock.Setup(value => value.Value).Returns("aaa"); //overwrite getter

            mock.Object.Value.ShouldBe("aaa"); // use Setup() returns

            mock.Object.Value = "aa"; // set a value
            //mock.Object.Value.ShouldBe("aa"); // 4.18.2 now! SetupAllProperties override default get/set
            mock.Object.Value.ShouldBe("aaa"); // use Setup() returns

            //override other Setup()
            mock.SetupAllProperties();

            mock.Object.Value.ShouldBeNull();
        }

        [Fact]
        public void PropValue_MoqDefaultValue_Test()
        {
            //moq property with default value
            var mock = new Mock<PropValue>();

            mock.SetupProperty(value => value.Id, 10);

            mock.Object.Id.ShouldBe(10);
        }

        [Fact]
        public void PropValue_MockPrivateProperty_Getter()
        {
            //moq protected virtual prop
            var mock = new Mock<PropValue>();

            mock.Protected().SetupGet<string>("PrivatePropForTest").Returns("value");

            //assert
            mock.Object.GetTest().ShouldBe("value");
            mock.Protected().VerifyGet<string>("PrivatePropForTest", Times.Once());
        }

        //[Fact]
        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        public void PropValue_MockPrivateProperty_Setter()
        {
            //moq protected virtual prop
            var mock = new Mock<PropValue>();

            var saved = "";
            //mock.Protected().SetupSet<string>("PrivatePropForTest", ItExpr.IsAny<string>()).Throws<ArgumentException>();
            mock.Protected().SetupSet<string>("PrivatePropForTest", "aaa")
                .Callback(s => saved = ""); //.Throws<ArgumentException>();
            mock.Protected().SetupSet<string>("PrivatePropForTest", "aa")
                .Callback(s => saved = s); //.Throws<ArgumentException>();

            mock.Object.SetTest("aa");
            mock.Object.SetTest("aaa");

            //todo: protected setter is wrong!!!
            //todo: https://github.com/moq/moq4/issues/1184
            saved.ShouldBe(""); // saved is "aaa"

            //assert
            mock.Protected().VerifySet<string>("PrivatePropForTest", Times.Exactly(2), ItExpr.IsAny<string>());
            mock.Protected().VerifySet<string>("PrivatePropForTest", Times.Once(), "aa"); // throw Moq.MockException
            mock.Protected().VerifySet<string>("PrivatePropForTest", Times.Once(),
                ItExpr.Is<string>(s => s == "aa")); // throw Moq.MockException
        }

        [Fact]
        public void PropValue_MockProtectedPropName_Test()
        {
            //arrange
            //var mock = new Mock<PropValue>();
            //mock.Setup(value => value.PropNameId).Returns(1);//throw NotSupportedException

            //moq protected property
            var propValue = Mock.Of<PropValue>(value => value.PropNameId == 1 &&
                value.PropName == Mock.Of<PropName>(name => name.Name == "prop name"));

            //assert
            propValue.PropNameId.ShouldBe(1);
            propValue.PropName.Name.ShouldBe("prop name");
        }
    }
}
