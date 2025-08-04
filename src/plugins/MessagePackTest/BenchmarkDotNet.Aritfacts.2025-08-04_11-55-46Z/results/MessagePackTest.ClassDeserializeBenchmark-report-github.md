```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.302
  [Host]     : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.7 (9.0.725.31616), X64 RyuJIT AVX2


```
| Method            | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------ |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MemoryPack        |  17.35 ns | 0.180 ns | 0.169 ns |  1.00 |    0.01 | 0.0043 |      56 B |        1.00 |
| MessagePackCSharp |  46.38 ns | 0.365 ns | 0.341 ns |  2.67 |    0.03 | 0.0042 |      56 B |        1.00 |
| Orleans           |  44.69 ns | 0.199 ns | 0.177 ns |  2.58 |    0.03 | 0.0042 |      56 B |        1.00 |
| OrleansReader     |  48.16 ns | 0.242 ns | 0.214 ns |  2.78 |    0.03 | 0.0042 |      56 B |        1.00 |
| SystemTextJson    | 309.52 ns | 1.036 ns | 0.918 ns | 17.85 |    0.17 | 0.0038 |      56 B |        1.00 |
