namespace WebApplicationTest;

public class SimpleOptions
{
    public ICollection<string> Parkings { get; set; } = new List<string>();
}

public class ReadOnlyOptions
{
    public IReadOnlyCollection<string> Parkings { get; set; } = null!; //Readonly 必须初始化为 null

    public IReadOnlyCollection<string> Codes { get; set; } = null!; //配置文件为空数组，属性会返回 null 而不是空数组
}

public class PrivateOptions
{
    public IReadOnlyCollection<string> Parkings { get; private set; } = null!; // = new List<string>();
}
