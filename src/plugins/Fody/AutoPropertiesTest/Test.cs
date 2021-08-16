using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoProperties;

namespace AutoPropertiesTest
{
    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class Test
    {
        [GetInterceptor]
        private T? GetValue<T>(string name, Type propertyType, PropertyInfo propertyInfo, FieldInfo fieldInfo, object fieldValue, T? genricFieldValue, ref T? refToBackingField)
        {
            return default;
        }

        [SetInterceptor]
        private void SetValue<T>(string name, Type propertyType, PropertyInfo propertyInfo, FieldInfo fieldInfo, object newValue, T? genricNewValue, out T? refToBackingField)
        {
            refToBackingField = default;
        }

        public string StrProp { get; set; }

        public string? StrNullProp { get; set; }

        public int IntProp { get; set; }

        public int? IntNullProp { get; set; }

        [InterceptIgnore]
        public string UntouchedProperty { get; set; }

        public Test(string strProp, string? strNullProp, int intProp, int? intNullProp, string untouchedProperty)
        {
            StrProp = strProp;
            StrNullProp = strNullProp;
            IntProp = intProp;
            IntNullProp = intNullProp;
            UntouchedProperty = untouchedProperty;
        }
    }
}
