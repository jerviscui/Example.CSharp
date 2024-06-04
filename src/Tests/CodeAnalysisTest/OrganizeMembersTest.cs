using System.Diagnostics.CodeAnalysis;

namespace CodeAnalysisTest;

/// <summary>
/// Organize members test case
/// </summary>
public class OrganizeMembersTest : IExplicit, IImplicit, IPropInterface
{
    [MemberNotNull(nameof(_strProp))]
    internal void SetProps()
    {
        BoolProp = true;
        PrivateProp = 1;
        ProtectedProp = PrivateProp;
        ProInternalProp = 3;
        InternalProp = 4;
        PublicProp = 5;
        StrProp = "Hello";
    }

    private bool _boolProp;
    public bool BoolProp
    {
        get => _boolProp;
        set => _boolProp = value && false;
    }

    private int _privateProp;
    private int PrivateProp
    {
        get => _privateProp;
        set => _privateProp = value + 1;
    }

    private int _protectedProp;
    protected int ProtectedProp
    {
        get => _protectedProp;
        set => _protectedProp = value + 1;
    }

    private int _proInternalProp;
    protected internal int ProInternalProp
    {
        get => _proInternalProp;
        set => _proInternalProp = value + 1;
    }

    private int _internalProp;
    internal int InternalProp
    {
        get => _internalProp;
        set => _internalProp = value + 1;
    }

    private int _publicProp;
    public int PublicProp
    {
        get => _publicProp;
        set => _publicProp = value + 1;
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

        [MemberNotNull(nameof(_strProp))]
        set => _strProp = $"{value}";
    }

    /// <summary>
    /// My field
    /// </summary>
    private readonly int _myField;

    static OrganizeMembersTest()
    {
        PublicStaticMehtod();
        PrivateStaticMehtod("Hello");
    }

    private OrganizeMembersTest()
    {
        SetProps();

        InterfaceProp = string.Empty;
        InterfaceProp2 = string.Empty;
        InterfaceProp1 = string.Empty;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="OrganizeMembersTest" /> class.
    /// </summary>
    /// <param name="myField">My field.</param>
    public OrganizeMembersTest(int myField) : this()
    {
        _myField = myField;
        _field1 = 0;
        _field2 = 0;

        PublicMehtod();
    }

    protected OrganizeMembersTest(string paramName) : this()
    {
        Console.WriteLine(paramName);
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

    /// <inheritdoc />
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
    /// protected mehtod.
    /// </summary>
    protected void ProtectedMehtod()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// protected internal mehtod.
    /// </summary>
    protected internal void ProInternalMethod()
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// Public static mehtod.
    /// </summary>
    public static void PublicStaticMehtod()
    {
        Field4 = "Hello";
    }

    /// <summary>
    /// Private static mehtod.
    /// </summary>
    /// <param name="hello"></param>
    private static void PrivateStaticMehtod(string hello)
    {
        Field4 = hello;
    }

    /// <summary>
    /// The field2
    /// </summary>
#pragma warning disable IDE0044 // Add readonly modifier
    private int _field2;
#pragma warning restore IDE0044 // Add readonly modifier

    public Task TaskMethod3Async(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task IImplicit.TaskMethodAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            goto rtag;
        }

        cancellationToken.ThrowIfCancellationRequested();
rtag:
        throw new NotImplementedException();
    }

    public Task TaskMethod2Async(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public string InterfaceProp { get; set; }

    /// <inheritdoc />
    public string InterfaceProp2 { get; set; }
    /// <inheritdoc />
    public string InterfaceProp1 { get; set; }
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
    public Task TaskMethod3Async(CancellationToken cancellationToken);

    /// <summary>
    /// Implicit method.
    /// </summary>
    /// <returns></returns>
    public Task TaskMethod2Async(CancellationToken cancellationToken);

    /// <summary>
    /// Interface has method and property.
    /// </summary>
    /// <value>
    /// The interface property.
    /// </value>
    public string InterfaceProp1 { get; set; }

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
