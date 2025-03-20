namespace CodeAnalysisTest;

public class IDisposableTest : IDisposable, IAsyncDisposable
{

    #region Constants & Statics

    private static BaseClassWithFinalizer ReturnDisposable()
    {
        return new BaseClassWithFinalizer();
    }

    private static async Task<BaseClassWithFinalizer> ReturnDisposableAsync()
    {
        await Task.Yield();
        return new BaseClassWithFinalizer();
    }

    public static void NewDisposableObject_Test()
    {
        // CA2000
        var a = new BaseClassWithFinalizer();
        //a.Dispose();
    }

    public static async Task ReturnDisposable_Async_Test()
    {
        // CA2000
        var a = await ReturnDisposableAsync();
        //await a.DisposeAsync();
    }

    public static void ReturnDisposable_Sync_Test()
    {
        // CA2000
        var a = ReturnDisposable();
        //a.Dispose();
    }

    #endregion

    // CA2213
    private readonly BaseClassWithFinalizer _field;

    private readonly IReturnDisposable _returnInterface;
    private readonly ReturnDisposableClass _returnClass;

    public IDisposableTest()
    {
        _field = new BaseClassWithFinalizer();
        _returnInterface = new ReturnDisposableClass();
        _returnClass = new ReturnDisposableClass();
    }

    #region IAsyncDisposable implementations

    public ValueTask DisposeAsync()
    {
        //await _field.DisposeAsync();
    }

    #endregion

    #region IDisposable implementations

    public void Dispose()
    {
        //_field.Dispose();
    }

    #endregion

    #region Methods

    public async Task Return_Class_Async_Test()
    {
        // CA2000
        var a = await _returnClass.ReturnDisposableAsync();
        //a.Dispose();
    }

    public void Return_Class_Sync_Test()
    {
        // CA2000
        var a = _returnClass.ReturnDisposable();
        //a.Dispose(); 
    }

    public async Task Return_Ingerface_Async_Test()
    {
        // interface 不会提示 CA2000
        var a = await _returnInterface.ReturnDisposableAsync();
    }

    public void Return_Ingerface_Sync_Test()
    {
        // interface 不会提示 CA2000
        var a = _returnInterface.ReturnDisposable();
    }

    public async Task Return_IngerfaceExtension_Async_Test()
    {
        // interface 不会提示 CA2000
        var a = await _returnInterface.ReturnDisposableAsyncWrap();
    }

    public void Return_IngerfaceExtension_Sync_Test()
    {
        // interface 不会提示 CA2000
        var a = _returnInterface.ReturnDisposableWrap();
    }

    #endregion

}

public static class IReturnDisposableExtensions
{

    #region Constants & Statics

    public static async Task<BaseClassWithFinalizer> ReturnDisposableAsyncWrap(this IReturnDisposable @return)
    {
        var a = await @return.ReturnDisposableAsync();
        return a;
    }

    public static BaseClassWithFinalizer ReturnDisposableWrap(this IReturnDisposable @return)
    {
        return @return.ReturnDisposable();
    }

    #endregion

}

public interface IReturnDisposable
{

    #region Methods

    public BaseClassWithFinalizer ReturnDisposable();

    public Task<BaseClassWithFinalizer> ReturnDisposableAsync();

    #endregion

}

public class ReturnDisposableClass : IReturnDisposable
{

    #region IReturnDisposable implementations

    public BaseClassWithFinalizer ReturnDisposable()
    {
        return new BaseClassWithFinalizer();
    }

    public async Task<BaseClassWithFinalizer> ReturnDisposableAsync()
    {
        await Task.Yield();
        return new BaseClassWithFinalizer();
    }

    #endregion

}

public class BaseClassWithFinalizer : IDisposable, IAsyncDisposable
{
    // To detect redundant calls
    private bool _disposed;

    ~BaseClassWithFinalizer() => Dispose(false);

    #region IAsyncDisposable implementations

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region IDisposable implementations

    // Public implementation of Dispose pattern callable by consumers.
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region Methods

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;
        if (disposing)
        {
            // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.
    }

    protected virtual ValueTask DisposeAsync(bool disposing)
    {
        if (_disposed)
        {
            return default;
        }

        _disposed = true;
        if (disposing)
        {
            // TODO: dispose managed state (managed objects).
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        return default;
    }

    #endregion

}
