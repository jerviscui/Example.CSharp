using System.ComponentModel.DataAnnotations;

namespace EfCoreTest
{
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
