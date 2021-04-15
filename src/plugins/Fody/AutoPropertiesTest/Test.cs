using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoProperties;

namespace AutoPropertiesTest
{
    public class Test
    {
        [GetInterceptor]
        private T? GetValue<T>(string name, Type propertyType, PropertyInfo propertyInfo, FieldInfo fieldInfo, object fieldValue, T? genricFieldValue, ref T? refToBackingField)
        {
            //todo cuizj: generic type default value
            return default;
        }

        [SetInterceptor]
        private void SetValue<T>(string name, Type propertyType, PropertyInfo propertyInfo, FieldInfo fieldInfo, object newValue, T? genricNewValue, out T? refToBackingField)
        {
            //todo cuizj: generic type default value
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
