using System.ComponentModel.DataAnnotations;

namespace EfCoreTest
{
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
}
