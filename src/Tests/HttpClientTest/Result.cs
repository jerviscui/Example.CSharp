namespace HttpClientTest;

internal class Result
{
    public object[] Data { get; set; }

    public int Code { get; set; }

    public string Message { get; set; }

    public bool Success { get; set; }
}

internal class Result<T>
{
    public T Data { get; set; }

    public int Code { get; set; }

    public string Message { get; set; }

    public bool Success { get; set; }
}
