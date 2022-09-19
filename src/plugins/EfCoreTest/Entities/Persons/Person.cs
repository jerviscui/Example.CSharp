namespace EfCoreTest
{
    public class Person : Entity
    {
        protected Person()
        {
        }

        public Person(long id, string name, long familyId, long? teacherId = null)
        {
            Id = id;
            Name = name;
            TeacherId = teacherId;
            FamilyId = familyId;

            SetProp(0, 0);
        }

        public Person(long id, string name, long familyId, long l = 0, decimal d = 0, long? teacherId = null) :
            this(id, name, familyId, teacherId)
        {
            SetProp(l, d);
        }

        public string Name { get; protected set; } = null!;

        public long? TeacherId { get; protected set; }

        public long FamilyId { get; protected set; }

        public long Long { get; private set; }

        public decimal Decimal { get; private set; }

        public void SetProp(long l, decimal d)
        {
            Long = l;
            Decimal = d;
        }
    }
}
