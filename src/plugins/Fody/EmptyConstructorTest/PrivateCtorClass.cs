namespace EmptyConstructorTest
{
    internal sealed class PrivateCtorClass
    {
        //Visibility='family' MakeExistingEmptyConstructorsVisible='True'
        //will change to protected
        private PrivateCtorClass()
        {
        }
    }
}
