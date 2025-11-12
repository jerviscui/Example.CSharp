using ObjectLayoutInspector;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StructTest;

internal static class StructTest
{

    #region Constants & Statics

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

    public static void Complex_Test()
    {
        var r = new Result<BaseError>(new BaseError());
        TypeLayout.PrintLayout(r.GetType(), true);
    }
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

//[StructLayout(LayoutKind.Auto, Pack = 1)]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "<Pending>")]
public readonly record struct Result<TError>
    where TError : struct
{

    #region Constants & Statics

    /// <summary>
    /// No errors, just return.
    /// </summary>
    private static readonly Result<TError> Ok = new(true);

    #endregion

    /// <summary>
    /// Gets the error.
    /// </summary>
    internal readonly TError _error;

    internal readonly bool _hasError;

#pragma warning disable IDE0060 // Remove unused parameter
    private Result(bool isOk)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        // just use for Ok
        _hasError = false;
    }

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

    #region Methods

    [UnscopedRef]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal ref readonly TError UnwrapErrorWithoutCheck()
    {
        Debug.Assert(_hasError, $"{nameof(_hasError)} is true");
        return ref _error;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error; otherwise <c>false</c>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool IsError()
    {
        return _hasError;
    }

    /// <summary>
    /// Determines whether this instance is error.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is error, and error must not be null; otherwise, <c>false</c>.
    /// </returns>
    [SuppressMessage(
        "Critical Code Smell",
        "S3874:\"out\" and \"ref\" parameters should not be used",
        Justification = "<Pending>")]
    public readonly bool IsError([NotNullWhen(true)] out TError? error)
    {
        error = null;

        if (_hasError)
        {
            error = _error;
            return true;
        }

        return false;
    }

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TError>(in TError error) => new(in error);
}

//[StructLayout(LayoutKind.Auto, Pack = 1)]
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly record struct BaseError(BaseErrorCode Code, string? Reason = null/*, Exception? Exception = null*/)
//: IBaseError<BaseError, BaseErrorCode>
{
    public BaseError() : this(BaseErrorCode.Failed)
    {
    }

    #region IBaseError implementations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<BaseError> Result(BaseErrorCode code, string? reason = null, Exception? exception = null)
    {
        return new BaseError(code, reason/*, exception*/);
    }

    #endregion

    #region IError implementations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<BaseError> Result()
    {
        return new BaseError();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<BaseError> Result(BaseErrorCode code)
    {
        return new BaseError(code);
    }

    #endregion

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator BaseError(BaseErrorCode errorCode) => new(errorCode);
}

public interface IError<TError, TCode>
    where TError : struct
{

    #region Constants & Statics

    /// <summary>
    /// Create a default result.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract Result<TError> Result();

    /// <summary>
    /// Create a result with the specified code.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract Result<TError> Result(TCode code);

    #endregion

    #region Properties

    /// <summary>
    /// Gets the error code.
    /// </summary>
    public TCode Code { get; }

    #endregion

}

public interface IBaseError<TError, TCode> : IError<TError, TCode>
    where TError : struct
{

    #region Constants & Statics

    /// <summary>
    /// Create a result with the specified code, reason and exception.
    /// </summary>
    /// <param name="code">The code.</param>
    /// <param name="reason">The reason.</param>
    /// <param name="exception">The exception.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static abstract Result<TError> Result(TCode code, string? reason = null, Exception? exception = null);

    #endregion

    #region Properties

    /// <summary>
    /// Gets the exception.
    /// </summary>
    public Exception? Exception { get; }

    /// <summary>
    /// Gets the reason.
    /// </summary>
    public string? Reason { get; }

    #endregion

}

public enum BaseErrorCode
{
    Failure = 0,

    Failed = 10,

    NotFound = 99
}

#endregion
