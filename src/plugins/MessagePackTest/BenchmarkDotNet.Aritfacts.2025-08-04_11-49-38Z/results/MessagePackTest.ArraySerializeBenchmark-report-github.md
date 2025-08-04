```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method                               | Mean        | Error     | StdDev    | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |------------:|----------:|----------:|-------:|--------:|-------:|----------:|------------:|
| MemoryPackSerialize                  |    331.6 ns |   4.08 ns |   3.41 ns |   1.00 |    0.01 | 0.9170 |   12032 B |        1.00 |
| MemoryPackSerializeUtf16             |    321.2 ns |   3.20 ns |   2.67 ns |   0.97 |    0.01 | 0.9170 |   12032 B |        1.00 |
| MessagePackSerialize                 | 10,936.0 ns |  61.25 ns |  57.29 ns |  32.98 |    0.36 | 1.2207 |   16032 B |        1.33 |
| OrleansSerialize                     |  9,299.0 ns |  76.95 ns |  71.98 ns |  28.05 |    0.35 | 1.2970 |   17032 B |        1.42 |
| SystemTextJsonSerialize              | 22,264.2 ns | 117.51 ns | 104.17 ns |  67.15 |    0.73 | 2.7161 |   35520 B |        2.95 |
|                                      |             |           |           |        |         |        |           |             |
| MemoryPackBufferWriter               |    124.9 ns |   1.42 ns |   1.26 ns |   1.00 |    0.01 |      - |         - |          NA |
| MemoryPackBufferWriterUtf16          |    126.9 ns |   2.43 ns |   2.27 ns |   1.02 |    0.02 |      - |         - |          NA |
| MessagePackBufferWriter              | 10,101.1 ns |  45.36 ns |  40.21 ns |  80.90 |    0.85 |      - |         - |          NA |
| OrleansWriterPooledArrayBufferWriter |  7,921.4 ns |  37.85 ns |  35.40 ns |  63.44 |    0.68 |      - |         - |          NA |
| OrleansWriterArrayBufferWriter       |  9,606.2 ns |  36.71 ns |  32.54 ns |  76.93 |    0.79 |      - |         - |          NA |
| OrleansPipeWriter                    |  9,848.7 ns |  36.64 ns |  34.27 ns |  78.87 |    0.81 | 0.0305 |     480 B |          NA |
| SystemTextJsonBufferWriter           | 23,848.9 ns | 118.42 ns | 110.77 ns | 191.00 |    2.05 | 2.4719 |   32312 B |          NA |
