```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method                               | Mean        | Error     | StdDev    | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------------------- |------------:|----------:|----------:|-------:|--------:|-------:|----------:|------------:|
| MemoryPackSerialize                  |    351.4 ns |   3.96 ns |   3.30 ns |   1.00 |    0.01 | 0.9170 |   12032 B |        1.00 |
| MemoryPackSerializeUtf16             |    348.1 ns |   5.57 ns |   5.21 ns |   0.99 |    0.02 | 0.9170 |   12032 B |        1.00 |
| MessagePackSerialize                 | 11,668.6 ns |  69.16 ns |  57.75 ns |  33.21 |    0.34 | 1.2207 |   16032 B |        1.33 |
| OrleansSerialize                     |  9,743.9 ns |  57.69 ns |  51.14 ns |  27.73 |    0.29 | 1.2970 |   17032 B |        1.42 |
| SystemTextJsonSerialize              | 23,521.3 ns | 120.68 ns | 100.77 ns |  66.95 |    0.67 | 2.7161 |   35521 B |        2.95 |
|                                      |             |           |           |        |         |        |           |             |
| MemoryPackBufferWriter               |    127.6 ns |   0.98 ns |   0.87 ns |   1.00 |    0.01 |      - |         - |          NA |
| MemoryPackBufferWriterUtf16          |    130.8 ns |   2.59 ns |   3.80 ns |   1.03 |    0.03 |      - |         - |          NA |
| MessagePackBufferWriter              | 10,616.3 ns |  59.05 ns |  55.24 ns |  83.23 |    0.69 |      - |         - |          NA |
| OrleansWriterPooledArrayBufferWriter |  8,631.8 ns |  63.93 ns |  59.80 ns |  67.67 |    0.63 |      - |         - |          NA |
| OrleansWriterArrayBufferWriter       |  9,850.8 ns |  68.35 ns |  57.07 ns |  77.22 |    0.67 |      - |         - |          NA |
| OrleansPipeWriter                    | 12,714.4 ns | 112.24 ns | 104.99 ns |  99.67 |    1.03 | 0.0305 |     480 B |          NA |
| SystemTextJsonBufferWriter           | 25,184.4 ns | 199.50 ns | 186.61 ns | 197.43 |    1.92 | 2.4719 |   32312 B |          NA |
