```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method                               | Mean       | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |-----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| MemoryPack                           |   3.586 ns | 0.0974 ns | 0.1083 ns |  1.00 |    0.04 | 0.0049 |      64 B |        1.00 |
| MessagePackCSharp                    |  33.212 ns | 0.6831 ns | 0.8389 ns |  9.27 |    0.36 | 0.0030 |      40 B |        0.62 |
| OrleansSerialize                     |  66.085 ns | 0.4841 ns | 0.4291 ns | 18.44 |    0.56 | 0.0036 |      48 B |        0.75 |
| SystemTextJson                       | 171.689 ns | 0.9768 ns | 0.9137 ns | 47.92 |    1.43 | 0.0317 |     416 B |        6.50 |
|                                      |            |           |           |       |         |        |           |             |
| MemoryPackBufferWriter               |   3.212 ns | 0.0225 ns | 0.0188 ns |  1.00 |    0.01 |      - |         - |          NA |
| MessagePackBufferWriter              |  40.043 ns | 0.1472 ns | 0.1377 ns | 12.47 |    0.08 |      - |         - |          NA |
| OrleansWriterPooledArrayBufferWriter |  46.411 ns | 0.1973 ns | 0.1845 ns | 14.45 |    0.10 |      - |         - |          NA |
| OrleansWriterArrayBufferWriter       |  33.667 ns | 0.1175 ns | 0.1099 ns | 10.48 |    0.07 |      - |         - |          NA |
| SystemTextJsonBufferWriter           | 127.652 ns | 0.6479 ns | 0.6060 ns | 39.74 |    0.29 | 0.0041 |      56 B |          NA |
