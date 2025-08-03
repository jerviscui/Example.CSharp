```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method                    | Mean        | Error     | StdDev    | Ratio  | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |------------:|----------:|----------:|-------:|--------:|-------:|-------:|----------:|------------:|
| MemoryPackDeserialize     |    355.2 ns |   4.49 ns |   4.20 ns |   1.00 |    0.02 | 0.9165 |      - |  11.74 KB |        1.00 |
| MessagePackDeserialize    | 16,391.7 ns | 164.43 ns | 153.80 ns |  46.15 |    0.67 | 0.9155 |      - |  11.74 KB |        1.00 |
| OrleansDeserialize        | 28,790.4 ns |  94.17 ns |  83.48 ns |  81.05 |    0.95 | 0.9155 |      - |  11.74 KB |        1.00 |
| OrleansReaderDeserialize  | 28,813.8 ns | 132.15 ns | 123.61 ns |  81.12 |    0.98 | 0.9155 |      - |  11.74 KB |        1.00 |
| SystemTextJsonDeserialize | 35,975.4 ns | 707.26 ns | 814.48 ns | 101.28 |    2.52 | 5.2490 | 0.2441 |  67.65 KB |        5.76 |
