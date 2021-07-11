using System;
using Moq;
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
        public void MockPropValue_AndPropName_Test()
        {
            //arrange
            //var mock = new Mock<PropValue>();
            //mock.Setup(value => value.PropNameId).Returns(1);//throw NotSupportedException

            var propValue = Mock.Of<PropValue>(value => value.PropNameId == 1 && 
                                                        value.PropName == Mock.Of<PropName>(name => name.Name == "prop name"));

            //act
            propValue.PropNameId.ShouldBe(1);
            propValue.PropName.Name.ShouldBe("prop name");
        }
    }
}