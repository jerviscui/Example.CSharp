namespace EmptyConstructorTest.Foos
{
    //ExcludeNamespaces='*.Foos*'
    internal sealed class Foo
    {
#pragma warning disable IDE0051 // 删除未使用的私有成员
        private Foo(int n)
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
        }
    }
}
