using System.Collections.Generic;

namespace EfCoreTest
{
    public class Tag
    {
        public string TagId { get; set; } = null!;

        public List<Post> Posts { get; set; } = new();
    }
}
