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

            SystemTextJsonTest.ProtectedProp_WithCtor_IsAssigned();
            SystemTextJsonTest.ProtectedProp_NoCtor_IsNotAssigned();
            SystemTextJsonTest.ProtectedProp_DeserializeWithJsonInclude_IsAssigned();
            SystemTextJsonTest.PrivateProp_DeserializeWithJsonInclude_IsAssigned();
            SystemTextJsonTest.GetonlyProp_DeserializeWithJsonInclude_IsNotAssigned();
            SystemTextJsonTest.GetonlyProp_DeserializeWithJsonConstructor_IsAssigned();
        }
    }
}
