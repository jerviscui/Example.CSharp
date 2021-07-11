namespace MoqTest.Domain.Prop
{
    public class PropValue : Entity
    {
        public string Value { get; protected set; }

        public PropName PropName { get; protected set; } = null!;

        public long PropNameId { get; protected set; }

#pragma warning disable 8618
        protected PropValue()
#pragma warning restore 8618
        {

        }

        public PropValue(long id, string value, PropName propName)
        {
            Id = id;
            Value = value;
            PropName = propName;
            PropNameId = propName.Id;
        }
    }
}