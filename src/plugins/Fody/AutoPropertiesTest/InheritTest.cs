using AutoProperties;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AutoPropertiesTest
{
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

        private static readonly StringLengthAttribute? LengthAttribute = typeof(InheritTest).GetProperty("Name")!.GetCustomAttribute<StringLengthAttribute>();

        [StringLength(5)]
        public string Name { get; set; }

        [InterceptIgnore]
        public string Text { get; set; }

        public InheritTest(string name, string text)
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
