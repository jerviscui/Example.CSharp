using Newtonsoft.Json;

namespace NewtonsoftJsonTest
{
    internal class ProtectedSetterClass1
    {
        public ProtectedSetterClass1(string s1, string s2)
        {
            S1 = s1;
            S2 = s2;
        }

        public string S1 { get; set; }

        public string S2 { get; protected set; }
    }

    internal class ProtectedSetterClass2
    {
        public string S3 { get; protected set; } = null!;

        public void SetS3()
        {
            S3 = "s3";
        }
    }

    [JsonObject(MemberSerialization.Fields)]
    internal class ProtectedSetterClass3
    {
        public string S3 { get; protected set; } = null!;

        public void SetS3()
        {
            S3 = "s3";
        }
    }

    internal class ProtectedSetterClass4
    {
        [JsonProperty]
        public string S3 { get; protected set; } = null!;

        public void SetS3()
        {
            S3 = "s3";
        }
    }

    internal class PrivateSetterClass
    {
        [JsonProperty]
        public string S3 { get; private set; } = null!;

        public void SetS3()
        {
            S3 = "s3";
        }
    }

    internal class GetonlyPropClass
    {
        public GetonlyPropClass(string s)
        {
            S3 = s;
        }

        [JsonConstructor]
        public GetonlyPropClass()
        {
        }

        [JsonProperty]
        public string S3 { get; }
    }

    [JsonObject(MemberSerialization.Fields)]
    internal class GetonlyPropClass2
    {
        public GetonlyPropClass2(string s)
        {
            S3 = s;
        }

        [JsonConstructor]
        public GetonlyPropClass2()
        {
        }

        public string S3 { get; }
    }

    public class DeserializeTest
    {
        public static void ProtectedProp_WithCtor_IsAssigned()
        {
            var c = new ProtectedSetterClass1("s1", "s2");

            var str = JsonConvert.SerializeObject(c);
            //"{\"S1\":\"s1\",\"S2\":\"s2\"}"

            var obj = (ProtectedSetterClass1)JsonConvert.DeserializeObject(str, typeof(ProtectedSetterClass1))!;

            //obj.S1 == "s1"
            //obj.S2 == "s2"
        }

        public static void ProtectedProp_NoCtor_IsNotAssigned()
        {
            var c = new ProtectedSetterClass2();
            c.SetS3();

            var str = JsonConvert.SerializeObject(c);
            //"{\"S3\":\"s3\"}"

            var obj = (ProtectedSetterClass2)JsonConvert.DeserializeObject(str, typeof(ProtectedSetterClass2))!;

            //obj.S3 is null
        }

        public static void ProtectedProp_DeserializeWithField_IsAssigned()
        {
            var c = new ProtectedSetterClass3();
            c.SetS3();

            var str = JsonConvert.SerializeObject(c);
            //"{\"<S3>k__BackingField\":\"s3\"}"

            var obj = (ProtectedSetterClass3)JsonConvert.DeserializeObject(str, typeof(ProtectedSetterClass3))!;

            //obj.S3 == "s3"
        }

        public static void ProtectedProp_DeserializeWithJsonProperty_IsAssigned()
        {
            var c = new ProtectedSetterClass4();
            c.SetS3();

            var str = JsonConvert.SerializeObject(c);
            //"{\"S3\":\"s3\"}"

            var obj = (ProtectedSetterClass4)JsonConvert.DeserializeObject(str, typeof(ProtectedSetterClass4))!;

            //obj.S3 == "s3"
        }

        public static void PrivateProp_DeserializeWithJsonProperty_IsAssigned()
        {
            var c = new PrivateSetterClass();
            c.SetS3();

            var str = JsonConvert.SerializeObject(c);
            //"{\"S3\":\"s3\"}"

            var obj = (PrivateSetterClass)JsonConvert.DeserializeObject(str, typeof(PrivateSetterClass))!;

            //obj.S3 == "s3"
        }

        public static void GetonlyProp_DeserializeWithJsonProperty_IsNotAssigned()
        {
            var c = new GetonlyPropClass("s3");

            var str = JsonConvert.SerializeObject(c);
            //"{\"S3\":\"s3\"}"

            var obj = (GetonlyPropClass)JsonConvert.DeserializeObject(str, typeof(GetonlyPropClass))!;

            //obj.S3 is null
        }

        public static void GetonlyProp_DeserializeWithField_IsAssigned()
        {
            var c = new GetonlyPropClass2("s3");

            var str = JsonConvert.SerializeObject(c);
            //"{\"<S3>k__BackingField\":\"s3\"}"

            var obj = (GetonlyPropClass2)JsonConvert.DeserializeObject(str, typeof(GetonlyPropClass2))!;

            //obj.S3 is null
        }
    }
}
