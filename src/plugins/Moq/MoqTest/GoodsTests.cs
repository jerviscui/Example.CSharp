using System;
using System.Collections.Generic;
using Moq;
using MoqTest.Domain.Goods;
using Xunit;

namespace MoqTest
{
    public class GoodsTests
    {
        [Fact]
        public void GoodsProps_Get_ReturnMock()
        {
            var mock = new Mock<Goods>();

        }
    }
}
