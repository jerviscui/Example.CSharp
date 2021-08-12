namespace NewtonsoftJsonTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DeserializeTest.ProtectedProp_DeserializeWithField_IsAssigned();
            DeserializeTest.ProtectedProp_DeserializeWithJsonProperty_IsAssigned();
        }
    }
}
