using Dolly;

namespace CloneTest;

[Clonable]
public partial class SimpleClass
{

    #region Properties

    [CloneIgnore]
    public float DontClone { get; set; }

    public required string First { get; set; }

    public int Second { get; set; }

    #endregion

}

[Clonable]
public partial record SimpleRecord(string First, int Second)
{

    #region Properties

    [CloneIgnore]
    public float DontClone { get; init; }

    #endregion

}

[Clonable]
public partial class ComplexClass
{
    public ComplexClass(SimpleRecord simpleRecord, SimpleClass simpleClass)
    {
        SimpleRecord = simpleRecord;
        SimpleClass = simpleClass;
    }

    #region Properties

    public SimpleClass SimpleClass { get; private set; }

    public SimpleRecord SimpleRecord { get; private set; }

    public int Third { get; set; }

    #endregion

}
