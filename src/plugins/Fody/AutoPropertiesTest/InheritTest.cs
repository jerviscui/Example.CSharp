using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoProperties;

namespace AutoPropertiesTest
{
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class InheritTest
    {
        [GetInterceptor]
        protected T Get<T>(ref T refToBackingField)
        {
            return refToBackingField;
        }

        [SetInterceptor]
        protected void CheckLength<T>(T genricNewValue, out T refToBackingField, PropertyInfo propertyInfo, string name)
        {
            if (propertyInfo.PropertyType != typeof(string))
            {
                throw new NotImplementedException();
            }

            var lengthAttribute = LengthAttribute;
            if (genricNewValue is string value && lengthAttribute is not null)
            {
                if (value.Length < lengthAttribute.MinimumLength)
                {
                    //throw new ArgumentException();
                    Console.WriteLine($"{name} length must >= {lengthAttribute.MinimumLength}");
                }

                if (value.Length > lengthAttribute.MaximumLength)
                {
                    Console.WriteLine($"{name} length must <= {lengthAttribute.MaximumLength}");
                }
            }

            refToBackingField = genricNewValue;
        }

        private static readonly StringLengthAttribute? LengthAttribute =
            typeof(InheritTest).GetProperty("Name")!.GetCustomAttribute<StringLengthAttribute>();

        [StringLength(5)]
        public string Name { get; set; }

        [InterceptIgnore]
        public string Text { get; set; }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        public InheritTest(string name, string text)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
        {
            //Name = name;
            //Text = text;
        }
    }

    public class Child : InheritTest
    {
        public string CName { get; set; }

        /// <inheritdoc />
        public Child(string name, string text, string cName) : base(name, text)
        {
            CName = cName;
        }
    }
}
