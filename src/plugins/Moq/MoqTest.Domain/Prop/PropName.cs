using System.Collections.Generic;

namespace MoqTest.Domain.Prop
{
    public class PropName : Entity
    {
#pragma warning disable 8618
        protected PropName()
#pragma warning restore 8618
        {
        }

        public PropName(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; protected set; }

        public List<PropValue> PropValues { get; protected set; } = new();
    }
}
