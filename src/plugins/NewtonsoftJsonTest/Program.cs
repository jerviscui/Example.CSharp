namespace NewtonsoftJsonTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            DeserializeTest.PrivateProp_DeserializeWithJsonProperty_IsAssigned();
            DeserializeTest.GetonlyProp_DeserializeWithJsonProperty_IsNotAssigned();
            DeserializeTest.GetonlyProp_DeserializeWithField_IsAssigned();
            DeserializeTest.ProtectedProp_DeserializeWithField_IsAssigned();
            DeserializeTest.ProtectedProp_DeserializeWithJsonProperty_IsAssigned();
        }
    }
}
