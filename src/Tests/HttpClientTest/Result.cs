namespace HttpClientTest;

internal sealed class Result
{
    public object[] Data { get; set; } = null!;

    public int Code { get; set; }

    public string Message { get; set; } = null!;

    public bool Success { get; set; }
}

internal sealed class Result<T>
{
    public T Data { get; set; } = default!;

    public int Code { get; set; }

    public string Message { get; set; } = null!;

    public bool Success { get; set; }
}
