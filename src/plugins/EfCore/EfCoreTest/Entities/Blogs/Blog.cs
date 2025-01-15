namespace EfCoreTest;

public class Blog : Entity, ISoftDelete
{
    /// <inheritdoc/>
    public Blog(long id, string title, string content)
    {
        Id = id;

        Title = title;
        Content = content;
        IsDelete = false;
    }

    #region Properties

    private readonly HashSet<BlogTag> _blogTags = [];

    // 一对多
    public IReadOnlyCollection<BlogTag> BlogTags => _blogTags;

    public string Content { get; set; }

    public bool IsDelete { get; set; }

    public string Title { get; set; }

    #endregion

    #region Methods

    public void AddTag(BlogTag tag)
    {
        _ = _blogTags.Add(tag);
    }

    public void RemoveTags()
    {
        _blogTags.Clear();
    }

    #endregion

}
