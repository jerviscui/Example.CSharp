```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.6093/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.301
  [Host]     : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.6 (9.0.625.26613), X64 RyuJIT AVX2


```
| Method                      | Mean        | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------------- |------------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| MemoryPackSerialize         |    382.7 ns |   7.53 ns |  10.56 ns |  1.00 |    0.04 | 0.9170 |   12032 B |        1.00 |
| MemoryPackSerializeUtf16    |    378.2 ns |   7.22 ns |   7.09 ns |  0.99 |    0.03 | 0.9170 |   12032 B |        1.00 |
| MessagePackSerialize        | 12,190.4 ns | 153.65 ns | 143.73 ns | 31.88 |    0.92 | 1.2207 |   16032 B |        1.33 |
| OrleansSerialize            |  9,692.6 ns | 136.67 ns | 127.84 ns | 25.35 |    0.75 | 1.2970 |   17032 B |        1.42 |
| SystemTextJsonSerialize     | 25,025.5 ns | 287.41 ns | 268.85 ns | 65.44 |    1.86 | 2.7161 |   35520 B |        2.95 |
|                             |             |           |           |       |         |        |           |             |
| MemoryPackBufferWriter      |    357.1 ns |   3.80 ns |   3.37 ns |  1.00 |    0.01 | 0.9170 |   12032 B |        1.00 |
| MemoryPackBufferWriterUtf16 |    357.6 ns |   4.52 ns |   4.01 ns |  1.00 |    0.01 | 0.9170 |   12032 B |        1.00 |
| MessagePackBufferWriter     | 10,998.7 ns |  29.65 ns |  24.75 ns | 30.81 |    0.29 |      - |         - |        0.00 |
| OrleansBufferWriter         |  8,518.1 ns |  57.47 ns |  50.95 ns | 23.86 |    0.26 |      - |         - |        0.00 |
| OrleansBufferWriter2        | 10,093.7 ns |  36.34 ns |  67.36 ns | 28.27 |    0.32 |      - |         - |        0.00 |
| OrleansPipeWriter           | 10,549.4 ns |  47.77 ns |  37.30 ns | 29.55 |    0.29 | 0.0305 |     480 B |        0.04 |
| SystemTextJsonBufferWriter  | 26,152.2 ns | 136.14 ns | 127.35 ns | 73.25 |    0.75 | 2.4719 |   32312 B |        2.69 |
