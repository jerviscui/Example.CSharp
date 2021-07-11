using System.Collections.Generic;
using System.Linq;

namespace MoqTest.Domain.Prop
{
    public class PropDomainService : IPropDomainService
    {
        private readonly IIdGenerator _idGenerator;
        private readonly IPropNameRepository _propNameRepository;

        public PropDomainService(IIdGenerator idGenerator, IPropNameRepository propNameRepository)
        {
            _idGenerator = idGenerator;
            _propNameRepository = propNameRepository;
        }

        public PropName CreatePropName(string name, IEnumerable<string>? values = null)
        {
            var propName = new PropName(_idGenerator.Create(), name);

            if (values == null)
            {
                return propName;
            }

            propName.PropValues.AddRange(values.Select(o => new PropValue(_idGenerator.Create(), o, propName)).ToArray());
            
            return propName;
        }

        public PropName AddPropValue(PropName propName, string value)
        {
            propName.PropValues.Add(new PropValue(_idGenerator.Create(), value, propName));

            return propName;
        }

        public PropName DeletePropValue(PropName propName, string value)
        {
            var propValue = propName.PropValues.First(o => o.Value == value);

            propName.PropValues.Remove(propValue);

            return propName;
        }

        public void DeletePropName(PropName propName)
        {
            propName.PropValues.Clear();

            _propNameRepository.DeletePropName(propName.Id);
        }
    }
}