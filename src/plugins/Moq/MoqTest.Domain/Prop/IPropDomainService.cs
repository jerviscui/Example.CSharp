using System.Collections.Generic;

namespace MoqTest.Domain.Prop
{
    public interface IPropDomainService
    {
        public PropName CreatePropName(string name, IEnumerable<string>? values = null);

        public PropName AddPropValue(PropName propName, string value);

        public PropName DeletePropValue(PropName propName, string value);

        public void DeletePropName(PropName propName);
    }
}