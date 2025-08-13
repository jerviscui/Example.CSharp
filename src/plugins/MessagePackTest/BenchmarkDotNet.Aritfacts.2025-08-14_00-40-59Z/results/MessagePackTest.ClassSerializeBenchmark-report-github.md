```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.304
  [Host]     : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2


```
| Method                               | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MemoryPack                           |  18.89 ns | 0.127 ns | 0.119 ns |  1.00 |    0.01 | 0.0049 |      64 B |        1.00 |
| MessagePackCSharp                    |  36.74 ns | 0.308 ns | 0.273 ns |  1.95 |    0.02 | 0.0030 |      40 B |        0.62 |
| OrleansSerialize                     |  71.60 ns | 0.502 ns | 0.470 ns |  3.79 |    0.03 | 0.0036 |      48 B |        0.75 |
| SystemTextJson                       | 171.18 ns | 1.574 ns | 1.473 ns |  9.06 |    0.09 | 0.0274 |     360 B |        5.62 |
| SystemTextJsonGenerator              | 114.91 ns | 0.696 ns | 0.651 ns |  6.08 |    0.05 | 0.0141 |     184 B |        2.88 |
|                                      |           |          |          |       |         |        |           |             |
| MemoryPackBufferWriter               |  20.64 ns | 0.182 ns | 0.170 ns |  1.00 |    0.01 |      - |         - |          NA |
| MessagePackBufferWriter              |  42.49 ns | 0.239 ns | 0.224 ns |  2.06 |    0.02 |      - |         - |          NA |
| OrleansWriterPooledArrayBufferWriter |  49.48 ns | 0.300 ns | 0.266 ns |  2.40 |    0.02 |      - |         - |          NA |
| OrleansWriterArrayBufferWriter       |  39.10 ns | 0.295 ns | 0.276 ns |  1.89 |    0.02 |      - |         - |          NA |
| SystemTextJsonBufferWriter           | 119.75 ns | 0.856 ns | 0.801 ns |  5.80 |    0.06 |      - |         - |          NA |
| SystemTextJsonBufferWriterGenerator  |  80.26 ns | 0.658 ns | 0.615 ns |  3.89 |    0.04 |      - |         - |          NA |
