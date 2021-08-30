using System.Collections.Generic;

namespace EfCoreTest
{
    public class Blog : Entity, ISoftDelete
    {
        /// <inheritdoc />
        public Blog(string title, string content)
        {
            Title = title;
            Content = content;
            IsDelete = false;
        }

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsDelete { get; set; }

        public List<BlogTag> BlogTags { get; set; } = new();
    }
}
