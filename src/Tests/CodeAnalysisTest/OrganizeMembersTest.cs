namespace CodeAnalysisTest;

/// <summary>
/// Organize members test case
/// </summary>
public class OrganizeMembersTest : IExplicit, IImplicit, IPropInterface
{
    private bool _boolProp;
    public bool BoolProp
    {
        get => _boolProp;
        set => _boolProp = value && false;
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
        set => _strProp = value + "";
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

        PublicMehtod();

        InterfaceProp = string.Empty;
        InterfaceProp2 = string.Empty;
    }

    // Field3
    private static readonly int Field3 = 1;
    private int _field1;
    private static string Field4 = string.Empty;
    /// <summary>
    /// Skip initialized fields
    /// </summary>
    public const string Const2 = "My Model";
    // one line comment
    private const int Const1 = 140;

    /// <inheritdoc/>
    public Task TaskMethodAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    private void PrivateMethod()
    {
        Console.WriteLine(_field1);
        Console.WriteLine(_field2);
        Console.WriteLine(Field3);
        Console.WriteLine(Field4);

        Console.WriteLine(BoolProp);
        Console.WriteLine(StrProp);
        Console.WriteLine(Const1);
        Console.WriteLine(Const2);
        Console.WriteLine(_myField);

        throw new NotImplementedException();
    }

    /// <summary>
    /// Public mehtod.
    /// </summary>
    public void PublicMehtod()
    {
        _field1 = 1;

        PublicStaticMehtod();
        PrivateMethod();
    }

    /// <summary>
    /// Public static mehtod.
    /// </summary>
    public static void PublicStaticMehtod()
    {
        Field4 = "Hello";
    }

    /// <summary>
    /// The field2
    /// </summary>
#pragma warning disable IDE0044 // Add readonly modifier
    private int _field2;
#pragma warning restore IDE0044 // Add readonly modifier

    Task IImplicit.TaskMethodAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public string InterfaceProp { get; set; }

    /// <inheritdoc/>
    public string InterfaceProp2 { get; set; }
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

    /// <summary>
    /// Interface has method and property.
    /// </summary>
    /// <value>
    /// The interface property.
    /// </value>
    public string InterfaceProp2 { get; set; }
}

/// <summary>
/// Prop Interface
/// </summary>
public interface IPropInterface
{
    /// <summary>
    /// Gets or sets the interface property.
    /// </summary>
    /// <value>
    /// The interface property.
    /// </value>
    public string InterfaceProp { get; set; }
}
