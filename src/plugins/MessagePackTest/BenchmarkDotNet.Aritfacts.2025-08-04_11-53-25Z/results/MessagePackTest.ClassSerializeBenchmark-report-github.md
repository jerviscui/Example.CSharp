```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method                               | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MemoryPack                           |  19.68 ns | 0.117 ns | 0.103 ns |  1.00 |    0.01 | 0.0049 |      64 B |        1.00 |
| MessagePackCSharp                    |  34.02 ns | 0.183 ns | 0.163 ns |  1.73 |    0.01 | 0.0030 |      40 B |        0.62 |
| OrleansSerialize                     |  70.77 ns | 0.270 ns | 0.239 ns |  3.60 |    0.02 | 0.0036 |      48 B |        0.75 |
| SystemTextJson                       | 165.97 ns | 0.766 ns | 0.716 ns |  8.43 |    0.06 | 0.0274 |     360 B |        5.62 |
|                                      |           |          |          |       |         |        |           |             |
| MemoryPackBufferWriter               |  19.87 ns | 0.075 ns | 0.067 ns |  1.00 |    0.00 |      - |         - |          NA |
| MessagePackBufferWriter              |  40.62 ns | 0.137 ns | 0.128 ns |  2.04 |    0.01 |      - |         - |          NA |
| OrleansWriterPooledArrayBufferWriter |  47.19 ns | 0.135 ns | 0.127 ns |  2.38 |    0.01 |      - |         - |          NA |
| OrleansWriterArrayBufferWriter       |  40.03 ns | 0.168 ns | 0.149 ns |  2.02 |    0.01 |      - |         - |          NA |
| SystemTextJsonBufferWriter           | 118.77 ns | 0.496 ns | 0.464 ns |  5.98 |    0.03 |      - |         - |          NA |
