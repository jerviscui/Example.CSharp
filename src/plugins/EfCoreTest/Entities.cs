using System.ComponentModel.DataAnnotations;

namespace EfCoreTest
{
    public class Entity
    {
        public long Id { get; protected set; }
    }

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

            Long = 0;
            Decimal = 0;
        }

        public Person(long id, string name, long familyId, long l = 0, decimal d = 0, long? teacherId = null)
        {
            Id = id;
            Name = name;
            TeacherId = teacherId;
            FamilyId = familyId;

            Long = l;
            Decimal = d;
        }

        public string Name { get; protected set; } = null!;

        public long? TeacherId { get; protected set; }

        public long FamilyId { get; protected set; }

        public long Long { get; private set; }

        public decimal Decimal { get; private set; }

        public void SetProp()
        {
            Long = 0;
            Decimal = 0;
        }
    }

    public class Family : Entity
    {
        public Family(long id, string? address = null, long? oldFamilyId = null)
        {
            Id = id;
            Address = address;
            OldFamilyId = oldFamilyId;
        }

        [StringLength(200)]
        public string? Address { get; protected set; }

        public long? OldFamilyId { get; protected set; }
    }

    public class Teacher : Entity
    {
        public Teacher(long id, string name)
        {
            Id = id;
            Name = name;
        }

        [StringLength(100)]
        public string Name { get; protected set; }
    }
}
