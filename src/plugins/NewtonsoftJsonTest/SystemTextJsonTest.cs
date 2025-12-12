using System.Text.Json;
using System.Text.Json.Serialization;

namespace NewtonsoftJsonTest;

public sealed class SystemTextJsonTest
{

    #region Constants & Statics

    public static void GetonlyProp_DeserializeWithJsonConstructor_IsAssigned()
    {
        var c = new GetonlyPropClass2("s3");

        var str = JsonSerializer.Serialize(c);
        //"{\"S3\":\"s3\"}"

        var obj = JsonSerializer.Deserialize<GetonlyPropClass2>(str)!;
        //要求构造函数参数列表必须和只读属性能够匹配，类型和名称（不区分大小写）
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

    public static void NumberHandling_Serialize_Test()
    {
        var number = new NumberClass();

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            NumberHandling =
                JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.WriteAsString
                | JsonNumberHandling.AllowNamedFloatingPointLiterals,
            WriteIndented = true
        };

        var str = JsonSerializer.Serialize(number, options);
        //{
        //  "dateTimeOffsetProp": "2019-01-02T03:04:05.006+08:00",
        //  "dateTimeProp": "2019-01-02T03:04:05.006007",
        //  "decimalProp": "79228162514264337593543950335",
        //  "doubleInfinityProp": "Infinity",
        //  "doubleNaNProp": "NaN",
        //  "doubleProp": "1.7976931348623157E+308",
        //  "floatNaNProp": "NaN",
        //  "floatProp": "3.4028235E+38",
        //  "intProp": "2147483647",
        //  "longNullProp": null,
        //  "longProp": "9223372036854775807",
        //  "timeSpanProp": "10675199.02:48:05.4775807"
        //}

        var data = JsonSerializer.Deserialize<NumberClass>(str, options);
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

    public static void ProtectedProp_DeserializeWithJsonInclude_IsAssigned()
    {
        var c = new ProtectedSetterClass3();
        c.SetS3();

        var str = JsonSerializer.Serialize(c);
        //"{\"S3\":\"s3\"}"

        var obj = JsonSerializer.Deserialize<ProtectedSetterClass3>(str)!;

        //obj.S3 == "s3"
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

    public static void ProtectedProp_WithCtor_IsAssigned()
    {
        var c = new ProtectedSetterClass1("s1", "s2");

        var str = JsonSerializer.Serialize(c);
        //"{\"S1\":\"s1\",\"S2\":\"s2\"}"

        var obj = JsonSerializer.Deserialize<ProtectedSetterClass1>(str)!;

        //obj.S1 == "s1"
        //obj.S2 == "s2"
    }

    #endregion

    public sealed class ProtectedSetterClass3
    {

        #region Properties

        [JsonInclude]
        public string S3 { get; protected set; } = null!;

        #endregion

        #region Methods

        public void SetS3()
        {
            S3 = "s3";
        }

        #endregion
    }

    public sealed class PrivateSetterClass
    {

        #region Properties

        [JsonInclude]
        public string S3 { get; private set; } = null!;

        #endregion

        #region Methods

        public void SetS3()
        {
            S3 = "s3";
        }

        #endregion
    }

    public sealed class GetonlyPropClass
    {
        [JsonConstructor]
        public GetonlyPropClass()
        {
        }

        public GetonlyPropClass(string s)
        {
            S3 = s;
        }

        #region Properties

        [JsonInclude]
        public string S3 { get; } = null!;

        #endregion
    }

    public sealed class GetonlyPropClass2
    {
        [JsonConstructor]
        public GetonlyPropClass2(string s3)
        {
            S3 = s3;
        }

        #region Properties

        [JsonInclude]
        public string S3 { get; }

        #endregion
    }

    public sealed class NumberClass
    {

        #region Properties

        public DateTimeOffset DateTimeOffsetProp { get; private set; } = new DateTimeOffset(
            2019,
            1,
            2,
            3,
            4,
            5,
            6,
            TimeSpan.FromHours(8));

        public DateTime DateTimeProp { get; private set; } = new DateTime(2019, 1, 2, 3, 4, 5, 6, 7);

        public decimal DecimalProp { get; private set; } = decimal.MaxValue;

        public double DoubleInfinityProp { get; private set; } = double.PositiveInfinity;

        public double DoubleNaNProp { get; private set; } = double.NaN;

        public double DoubleProp { get; private set; } = double.MaxValue;

        public float FloatNaNProp { get; private set; } = float.NaN;

        public float FloatProp { get; private set; } = float.MaxValue;

        public int IntProp { get; private set; } = int.MaxValue;

        public long? LongNullProp { get; private set; }

        public long LongProp { get; private set; } = long.MaxValue;

        public TimeSpan TimeSpanProp { get; private set; } = TimeSpan.MaxValue;

        #endregion
    }
}
