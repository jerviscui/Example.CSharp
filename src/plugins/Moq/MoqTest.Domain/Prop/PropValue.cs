﻿namespace MoqTest.Domain.Prop
{
    public class PropValue : Entity
    {
        public virtual string Value { get; set; }

        protected virtual string PrivatePropForTest { get; set; } = null!;

        public void SetTest(string s)
        {
            PrivatePropForTest = s;
        }

        public string GetTest()
        {
            return PrivatePropForTest;
        }

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

        public void SetId()
        {
            Id = 1;
        }

        public void SetPropNameId(PropValue a)
        {
            a.PropNameId = 1;
        }
    }
}