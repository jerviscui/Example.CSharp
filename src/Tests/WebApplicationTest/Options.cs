namespace WebApplicationTest;

public class SimpleOptions
{

    #region Properties

    public IEnumerable<string> InitArray { get; set; } = []; //Wrong！无法通过配置文件赋值

    public string[] InitArray2 { get; set; } = [];

    public string[] InitArray3 { get; set; } = null!; //禁止这样初始化，配置文件为空数组时属性会返回 null

    public ICollection<string> Parkings { get; set; } = [];

    #endregion

}

public class ReadOnlyOptions
{

    #region Properties

    public IReadOnlyCollection<string> Codes { get; set; } = null!; //禁止这样初始化，配置文件为空数组时属性会返回 null

    public IReadOnlyCollection<string> Parkings { get; set; } = [];

    #endregion

    //Readonly 必须初始化为 null
}

public record PrivateOptions
{

    #region Properties

    public IReadOnlyCollection<string> Parkings { get; private set; } = null!; // 可以支持 private set

    #endregion

}
