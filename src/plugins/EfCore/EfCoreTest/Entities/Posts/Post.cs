namespace EfCoreTest;

public class Post
{
    private readonly List<Tag> _tags = [];

    #region Properties

    public string? Content { get; set; }

    public int PostId { get; set; }

    // 多对多
    public IReadOnlyCollection<Tag> Tags
    {
        get => _tags.AsReadOnly();
        protected set => _tags.AddRange(value.AsEnumerable());
    }

    public string? Title { get; set; }

    #endregion

    #region Methods

    public void AddTag(Tag tag)
    {
        _tags.Add(tag);
    }

    #endregion

}
