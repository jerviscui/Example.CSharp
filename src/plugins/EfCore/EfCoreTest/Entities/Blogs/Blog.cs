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

    //private readonly List<BlogTag> _blogTags = [];
    private readonly HashSet<BlogTag> _blogTags = [];

    //public IReadOnlyCollection<BlogTag> BlogTags
    //{
    //    get => _blogTags.AsReadOnly();
    //    set => _blogTags.AddRange(value);
    //}

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
