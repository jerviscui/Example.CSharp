using ObjectLayoutInspector;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace StructTest;

internal static class StructTest
{

    #region Constants & Statics

    public static void Complex_Test()
    {
        var r = new Result<BaseError>(new BaseError(11, null, null));
        TypeLayout.PrintLayout(r.GetType(), true);
    }

    public static void Simple_Test()
    {
        var box = new BoxStruct(false, 0, new InnerStruct(100, true));

        TypeLayout.PrintLayout(box.GetType(), true);

        // LayoutKind.Auto
        //Type layout for 'BoxStruct'
        //Size: 16 bytes. Paddings: 5 bytes (%31 of empty space)
        //|=========================================|
        //|   0-3: Int32 _intField (4 bytes)        |
        //|-----------------------------------------|
        //|     4: Boolean _boolField (1 byte)      |
        //|-----------------------------------------|
        //|   5-7: padding (3 bytes)                |
        //|-----------------------------------------|
        //|  8-15: InnerStruct _inner (8 bytes)     |
        //| |=====================================| |
        //| |   0-3: Int32 _intField (4 bytes)    | |
        //| |-------------------------------------| |
        //| |     4: Boolean _boolField2 (1 byte) | |
        //| |-------------------------------------| |
        //| |     5: Boolean _boolField (1 byte)  | |
        //| |-------------------------------------| |
        //| |   6-7: padding (2 bytes)            | |
        //| |=====================================| |
        //|=========================================|

        //LayoutKind.Sequential
        //Type layout for 'BoxStruct'
        //Size: 20 bytes. Paddings: 9 bytes (%45 of empty space)
        //|=========================================|
        //|     0: Boolean _boolField (1 byte)      |
        //|-----------------------------------------|
        //|   1-3: padding (3 bytes)                |
        //|-----------------------------------------|
        //|   4-7: Int32 _intField (4 bytes)        |
        //|-----------------------------------------|
        //|  8-19: InnerStruct _inner (12 bytes)    |
        //| |=====================================| |
        //| |     0: Boolean _boolField2 (1 byte) | |
        //| |-------------------------------------| |
        //| |   1-3: padding (3 bytes)            | |
        //| |-------------------------------------| |
        //| |   4-7: Int32 _intField (4 bytes)    | |
        //| |-------------------------------------| |
        //| |     8: Boolean _boolField (1 byte)  | |
        //| |-------------------------------------| |
        //| |  9-11: padding (3 bytes)            | |
        //| |=====================================| |
        //|=========================================|
    }

    #endregion

}

#region Simple Structs

[StructLayout(LayoutKind.Sequential)]
public readonly record struct BoxStruct
{
    public readonly bool _boolField;
    public readonly int _intField;
    public readonly InnerStruct _inner;

    public BoxStruct(bool boolField, int intField, InnerStruct inner)
    {
        _boolField = boolField;
        _intField = intField;
        _inner = inner;
    }
}

[StructLayout(LayoutKind.Sequential)]
public readonly record struct InnerStruct
{
    public readonly bool _boolField2;
    public readonly int _intField;
    public readonly bool _boolField;

    public InnerStruct(int intField, bool boolField)
    {
        _intField = intField;
        _boolField = boolField;
    }
}

#endregion

#region Complex

[StructLayout(LayoutKind.Auto, Pack = 1)]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public readonly record struct Result<TError>
    where TError : struct
{
    /// <summary>
    /// Gets the error.
    /// </summary>
    internal readonly TError _error;

    internal readonly bool _hasError;

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TError}"/> with default <typeparamref name="TError"/>.
    /// </summary>
    public Result() : this(new TError())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TData, TError}"/> with error.
    /// The result is failed.
    /// </summary>
    /// <param name="error">The error.</param>
    public Result(in TError error)
    {
        _error = error;
        _hasError = true;
    }
}

[StructLayout(LayoutKind.Auto, Pack = 1)]
//[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct BaseError
{
    public readonly int Code;

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public readonly Exception? Exception;

    /// <summary>
    /// Gets the reason.
    /// </summary>
    public readonly string? Reason;

    public BaseError(int code, Exception? exception, string? reason)
    {
        Code = code;
        //Exception = exception;
        Reason = reason;
    }
}

#endregion
