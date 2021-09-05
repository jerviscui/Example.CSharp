using System.Collections.Generic;
using System.Linq;

namespace EfCoreTest
{
    public class Post
    {
        public int PostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public IReadOnlyCollection<Tag> Tags
        {
            get => _tags.AsReadOnly();
            protected set => _tags.AddRange(value.AsEnumerable());
        }

        private readonly List<Tag> _tags = new();

        public void AddTag(Tag tag)
        {
            _tags.Add(tag);
        }
    }
}
