namespace EfCoreTest;

public class BlogTag : Entity, ISoftDelete
{
    public BlogTag(long id, long blogId, string tagId)
    {
        Id = id;

        BlogId = blogId;
        TagId = tagId;
        IsDelete = false;
    }

    #region Properties

    public long BlogId { get; set; }

    public bool IsDelete { get; set; }

    public string TagId { get; set; }

    #endregion

}
