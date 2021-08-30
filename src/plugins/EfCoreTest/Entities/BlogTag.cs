namespace EfCoreTest
{
    public class BlogTag : Entity, ISoftDelete
    {
        public BlogTag(long blogId, string tagId)
        {
            BlogId = blogId;
            TagId = tagId;
            IsDelete = false;
        }

        public long BlogId { get; set; }

        public string TagId { get; set; }

        public bool IsDelete { get; set; }
    }
}
