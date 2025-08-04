```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method                    | Mean        | Error     | StdDev    | Ratio  | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|-------------------------- |------------:|----------:|----------:|-------:|--------:|-------:|-------:|----------:|------------:|
| MemoryPackDeserialize     |    341.0 ns |   5.13 ns |   4.80 ns |   1.00 |    0.02 | 0.9165 |      - |  11.74 KB |        1.00 |
| MessagePackDeserialize    | 16,319.5 ns |  78.98 ns |  73.88 ns |  47.87 |    0.68 | 0.9155 |      - |  11.74 KB |        1.00 |
| OrleansDeserialize        | 29,012.3 ns |  72.45 ns |  67.77 ns |  85.11 |    1.17 | 0.9155 |      - |  11.74 KB |        1.00 |
| OrleansReaderDeserialize  | 29,124.0 ns | 108.37 ns | 101.37 ns |  85.44 |    1.19 | 0.9155 |      - |  11.74 KB |        1.00 |
| SystemTextJsonDeserialize | 34,624.7 ns | 191.43 ns | 179.06 ns | 101.57 |    1.47 | 5.2490 | 0.2441 |  67.65 KB |        5.76 |
