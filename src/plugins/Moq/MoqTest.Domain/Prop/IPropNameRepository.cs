using System;

namespace MoqTest.Domain.Prop
{
    public class PropNameRepository
    {
        public virtual bool DeletePropName(long id)
        {
            throw new NotImplementedException();
        }

        protected virtual bool PrivateMethodForTest(long id)
        {
            throw new NotImplementedException();
        }

        public bool Test()
        {
            return PrivateMethodForTest(1);
        }
    }

    public interface IPropNameRepository
    {
        public bool DeletePropName(long id);

        protected bool PrivateMethodForTest(long id);

        public bool Test()
        {
            return PrivateMethodForTest(1);
        }
    }
}
