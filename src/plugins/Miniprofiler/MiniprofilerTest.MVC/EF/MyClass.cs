using System.ComponentModel.DataAnnotations;

namespace MiniprofilerTest.MVC;

[Serializable]
public class MyClass
{
    public MyClass(string name) => Name = name;

    [Key]
    public long Id { get; set; }

    public string Name { get; set; }
}
