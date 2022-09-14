using System.Collections.Generic;

namespace EfCoreTest
{
    public class Tag
    {
        public string TagId { get; set; }

        public List<Post> Posts { get; set; } = new();
    }
}
