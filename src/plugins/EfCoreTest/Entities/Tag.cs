using System.Collections.Generic;

namespace EfCoreTest
{
    public class Tag
    {
        public string TagId { get; set; }

        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
