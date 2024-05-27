namespace CodeAnalysisTest;

/// <summary>
/// Organize members test case
/// </summary>
public class OrganizeMembersTest : IExplicit, IImplicit
{
    private bool _boolProp;
    public bool BoolProp
    {
        get => _boolProp;
        set => _boolProp = value;
    }

    /// <summary>
    /// The string property
    /// </summary>
    private string _strProp;
    /// <summary>
    /// Gets or sets the string property.
    /// </summary>
    /// <value>
    /// The string property.
    /// </value>
    public string StrProp
    {
        get => _strProp;
        set => _strProp = value;
    }

    /// <summary>
    /// My field
    /// </summary>
    private readonly int _myField;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizeMembersTest"/> class.
    /// </summary>
    /// <param name="boolProp">if set to <c>true</c> [bool property].</param>
    /// <param name="strProp">The string property.</param>
    /// <param name="myField">My field.</param>
    public OrganizeMembersTest(bool boolProp, string strProp, int myField)
    {
        _boolProp = boolProp;
        _strProp = strProp;
        _myField = myField;

        _field1 = 0;
        _field2 = 0;
    }

    // Field3
    private static readonly int Field3 = 1;
    private int _field1;
    private static string Field4 = string.Empty;
    /// <summary>
    /// Skip initialized fields
    /// </summary>
    private const string Const2 = "My Model";
    // one line comment
    private const int Const1 = 140;


    /// <inheritdoc/>
    public Task TaskMethodAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private void MethodName()
    {
        Console.WriteLine(_field1);
        Console.WriteLine(_field2);
        Console.WriteLine(Field3);
        Console.WriteLine(Field4);

        throw new NotImplementedException();
    }
    /// <summary>
    /// The field2
    /// </summary>
    private int _field2;


    Task IImplicit.TaskMethodAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Explicit interface
/// </summary>
public interface IExplicit
{
    /// <summary>
    /// Explicit method.
    /// </summary>
    /// <returns></returns>
    public Task TaskMethodAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Implicit interface
/// </summary>
public interface IImplicit
{
    /// <summary>
    /// Implicit method.
    /// </summary>
    /// <returns></returns>
    public Task TaskMethodAsync(CancellationToken cancellationToken);
}
