```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.304
  [Host]     : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2


```
| Method                  | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|------------------------ |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| MemoryPack              |  17.49 ns | 0.128 ns | 0.113 ns |  1.00 |    0.01 | 0.0043 |      56 B |        1.00 |
| MessagePackCSharp       |  48.65 ns | 0.333 ns | 0.312 ns |  2.78 |    0.02 | 0.0042 |      56 B |        1.00 |
| Orleans                 |  47.95 ns | 0.550 ns | 0.487 ns |  2.74 |    0.03 | 0.0042 |      56 B |        1.00 |
| OrleansReader           |  45.77 ns | 0.301 ns | 0.282 ns |  2.62 |    0.02 | 0.0042 |      56 B |        1.00 |
| SystemTextJson          | 323.23 ns | 4.782 ns | 4.473 ns | 18.48 |    0.27 | 0.0038 |      56 B |        1.00 |
| SystemTextJsonGenerator | 300.46 ns | 1.988 ns | 1.860 ns | 17.18 |    0.15 | 0.0038 |      56 B |        1.00 |
