using System.Text;

namespace RecordTest
{
    internal record Base(string Name);

    internal record Derived(string Name = "") : Base(Name)
    {
        //C# 10 语法 sealed ToString
        /// <inheritdoc />
        public sealed override string ToString() => base.ToString();
    }

    internal record Child : Derived
    {
        /// <inheritdoc />
        protected override bool PrintMembers(StringBuilder builder) => base.PrintMembers(builder);
    }
}
