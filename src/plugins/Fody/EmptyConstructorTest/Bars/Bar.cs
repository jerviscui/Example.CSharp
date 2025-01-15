namespace EmptyConstructorTest.Bars
{
    internal sealed class Bar
    {
#pragma warning disable IDE0051 // 删除未使用的私有成员
        private Bar(int n)
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
        }

        //generate
        //protected Bar()
        //{
        //}
    }
}
