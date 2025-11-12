```

BenchmarkDotNet v0.15.2, Windows 10 (10.0.19045.5487/22H2/2022Update)
Intel Xeon CPU E5-2667 v4 3.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.304
  [Host]    : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2
  MediumRun : .NET 9.0.8 (9.0.825.36511), X64 RyuJIT AVX2

Job=MediumRun  IterationCount=15  LaunchCount=1  
WarmupCount=10  

```
| Method                           | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------------------------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|------------:|
| DerivedClass_Property            | 0.3260 ns | 0.0403 ns | 0.0377 ns | 0.3120 ns |     ? |       ? |         - |           ? |
| ExtendedClass_AdditionalProperty | 0.0000 ns | 0.0000 ns | 0.0000 ns | 0.0000 ns |     ? |       ? |         - |           ? |
| ExtendedClass_StandardProperty   | 0.0051 ns | 0.0059 ns | 0.0056 ns | 0.0017 ns |     ? |       ? |         - |           ? |
| SealedClass_Property             | 0.0117 ns | 0.0142 ns | 0.0126 ns | 0.0081 ns |     ? |       ? |         - |           ? |
| StandardClass_Property           | 0.0070 ns | 0.0096 ns | 0.0085 ns | 0.0038 ns |     ? |       ? |         - |           ? |
