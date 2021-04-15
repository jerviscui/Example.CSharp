using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoProperties;

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
        protected void CheckLength<T>(T genricNewValue, out T refToBackingField)
        {
            if (genricNewValue is string value)
            {
                if (value.Length == 0)
                {
                    throw new ArgumentException();
                }
            }

            refToBackingField = genricNewValue;
        }

        public string Name { get; set; }

        [InterceptIgnore]
        public string Text { get; set; }

        public InheritTest(string name, string text)
        {
            Name = name;
            Text = text;
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
