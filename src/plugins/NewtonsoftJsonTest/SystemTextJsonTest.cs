using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewtonsoftJsonTest
{
    internal class SystemTextJsonTest
    {
        internal class ProtectedSetterClass3
        {
            [JsonInclude]
            public string S3 { get; protected set; } = null!;

            public void SetS3()
            {
                S3 = "s3";
            }
        }

        internal class PrivateSetterClass
        {
            [JsonInclude]
            public string S3 { get; private set; } = null!;

            public void SetS3()
            {
                S3 = "s3";
            }
        }

        internal class GetonlyPropClass
        {
            [JsonInclude]
            public string S3 { get; } = null!;

            public GetonlyPropClass(string s)
            {
                S3 = s;
            }

            [JsonConstructor]
            public GetonlyPropClass()
            {
            }
        }

        internal class GetonlyPropClass2
        {
            [JsonInclude]
            public string S3 { get; }

            [JsonConstructor]
            public GetonlyPropClass2(string s3)
            {
                S3 = s3;
            }
        }

        public static void ProtectedProp_WithCtor_IsAssigned()
        {
            var c = new ProtectedSetterClass1("s1", "s2");

            var str = JsonSerializer.Serialize(c);
            //"{\"S1\":\"s1\",\"S2\":\"s2\"}"

            var obj = JsonSerializer.Deserialize<ProtectedSetterClass1>(str)!;

            //obj.S1 == "s1"
            //obj.S2 == "s2"
        }

        public static void ProtectedProp_NoCtor_IsNotAssigned()
        {
            var c = new ProtectedSetterClass2();
            c.SetS3();

            var str = JsonSerializer.Serialize(c);
            //"{\"S3\":\"s3\"}"

            var obj = JsonSerializer.Deserialize<ProtectedSetterClass2>(str)!;

            //obj.S3 is null
        }

        public static void ProtectedProp_DeserializeWithJsonInclude_IsAssigned()
        {
            var c = new ProtectedSetterClass3();
            c.SetS3();

            var str = JsonSerializer.Serialize(c);
            //"{\"S3\":\"s3\"}"

            var obj = JsonSerializer.Deserialize<ProtectedSetterClass3>(str)!;

            //obj.S3 == "s3"
        }

        public static void PrivateProp_DeserializeWithJsonInclude_IsAssigned()
        {
            var c = new PrivateSetterClass();
            c.SetS3();

            var str = JsonSerializer.Serialize(c);
            //"{\"S3\":\"s3\"}"

            var obj = JsonSerializer.Deserialize<PrivateSetterClass>(str)!;

            //obj.S3 == "s3"
        }

        public static void GetonlyProp_DeserializeWithJsonInclude_IsNotAssigned()
        {
            var c = new GetonlyPropClass("s3");

            var str = JsonSerializer.Serialize(c);
            //"{\"S3\":\"s3\"}"

            var obj = JsonSerializer.Deserialize<GetonlyPropClass>(str)!;

            //obj.S3 is null
        }

        public static void GetonlyProp_DeserializeWithJsonConstructor_IsAssigned()
        {
            var c = new GetonlyPropClass2("s3");

            var str = JsonSerializer.Serialize(c);
            //"{\"S3\":\"s3\"}"

            var obj = JsonSerializer.Deserialize<GetonlyPropClass2>(str)!;
            //要求构造函数参数列表必须和只读属性能够匹配，类型和名称（不区分大小写）
            //obj.S3 == "s3"
        }
    }
}
