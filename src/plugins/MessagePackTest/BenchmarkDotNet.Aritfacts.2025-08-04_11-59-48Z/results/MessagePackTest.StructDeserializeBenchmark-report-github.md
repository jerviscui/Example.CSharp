```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method            | Mean       | Error     | StdDev    | Ratio  | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |-----------:|----------:|----------:|-------:|--------:|-------:|----------:|------------:|
| MemoryPack        |   2.332 ns | 0.0085 ns | 0.0075 ns |   1.00 |    0.00 |      - |         - |          NA |
| MessagePackCSharp |  46.150 ns | 0.1266 ns | 0.1184 ns |  19.79 |    0.08 |      - |         - |          NA |
| Orleans           |  36.003 ns | 0.1688 ns | 0.1579 ns |  15.44 |    0.08 |      - |         - |          NA |
| OrleansReader     |  35.752 ns | 0.0901 ns | 0.0704 ns |  15.33 |    0.06 |      - |         - |          NA |
| SystemTextJson    | 275.311 ns | 1.4188 ns | 1.1848 ns | 118.04 |    0.61 | 0.0038 |      56 B |          NA |
