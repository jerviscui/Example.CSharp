using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace CollectionTest;

//[Orderer(SummaryOrderPolicy.FastestToSlowest)]
//[RankColumn(NumeralSystem.Arabic)]
//[MarkdownExporter]
[MemoryDiagnoser]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
public class FrozenCollectionBenchmark
{

    #region Constants & Statics

    private const int itemsCount = 100_000;
    private const int keyToFind = 500;

    #endregion

    private readonly Dictionary<int, int> _dictionary = Enumerable.Range(0, itemsCount).ToDictionary(key => key);

    private readonly FrozenDictionary<int, int> _frozenDictionary = Enumerable.Range(0, itemsCount)
        .ToFrozenDictionary(key => key);
    private readonly FrozenSet<int> _frozenSet = Enumerable.Range(0, itemsCount).ToFrozenSet();

    private readonly HashSet<int> _hashSet = Enumerable.Range(0, itemsCount).ToHashSet();
    private readonly ImmutableDictionary<int, int> _immutableDictionary = Enumerable.Range(0, itemsCount)
        .ToImmutableDictionary(key => key);
    private readonly ImmutableHashSet<int> _immutableHashSet = Enumerable.Range(0, itemsCount).ToImmutableHashSet();
    private readonly List<int> _list = Enumerable.Range(0, itemsCount).ToList();

    #region GetValue

    [Benchmark]
    [BenchmarkCategory("GetValue")]
    public void TryGetValueDictionary()
    {
        _ = _dictionary.TryGetValue(keyToFind, out _);
    }

    [Benchmark]
    [BenchmarkCategory("GetValue")]
    public void TryGetValueFrozenDictionary()
    {
        _ = _frozenDictionary.TryGetValue(keyToFind, out _);
    }

    [Benchmark]
    [BenchmarkCategory("GetValue")]
    public void TryGetValueFrozenSet()
    {
        _ = _frozenSet.TryGetValue(keyToFind, out _);
    }

    [Benchmark]
    [BenchmarkCategory("GetValue")]
    public void TryGetValueHashSet()
    {
        _ = _hashSet.TryGetValue(keyToFind, out _);
    }

    [Benchmark]
    [BenchmarkCategory("GetValue")]
    public void TryGetValueImmutableDictionary()
    {
        _ = _immutableDictionary.TryGetValue(keyToFind, out _);
    }

    [Benchmark]
    [BenchmarkCategory("GetValue")]
    public void TryGetValueImmutableHashSet()
    {
        _ = _immutableHashSet.TryGetValue(keyToFind, out _);
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("GetValue")]
    public void TryGetValueList()
    {
        _ = _list.FirstOrDefault(o => o == keyToFind);
    }

    #endregion

    #region Create

    [Benchmark]
    [BenchmarkCategory("Create")]
    public void CreateDictionary()
    {
        var dictionary = Enumerable.Range(0, itemsCount).ToDictionary(key => key);
    }

    [Benchmark]
    [BenchmarkCategory("Create")]
    public void CreateFrozenDictionary()
    {
        var frozenDictionary = Enumerable.Range(0, itemsCount).ToFrozenDictionary(key => key);
    }

    [Benchmark]
    [BenchmarkCategory("Create")]
    public void CreateFrozenSet()
    {
        _ = Enumerable.Range(0, itemsCount).ToFrozenSet();
    }

    [Benchmark]
    [BenchmarkCategory("Create")]
    public void CreateHashSet()
    {
        _ = Enumerable.Range(0, itemsCount).ToHashSet();
    }

    [Benchmark]
    [BenchmarkCategory("Create")]
    public void CreateImmutableDictionary()
    {
        var dictionary = Enumerable.Range(0, itemsCount).ToImmutableDictionary(key => key);
    }

    [Benchmark]
    [BenchmarkCategory("Create")]
    public void CreateImmutableHashSet()
    {
        _ = Enumerable.Range(0, itemsCount).ToImmutableHashSet();
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Create")]
    public void CreateList()
    {
        _ = Enumerable.Range(0, itemsCount).ToList();
    }

    #endregion

    #region Foreach

    [Benchmark]
    [BenchmarkCategory("Foreach")]
    public void ForeachDictionary()
    {
        foreach (var item in _dictionary)
        {
            _ = item.Key;
        }
    }

    [Benchmark]
    [BenchmarkCategory("Foreach")]
    public void ForeachFrozenDictionary()
    {
        foreach (var item in _frozenDictionary)
        {
            _ = item.Key;
        }
    }

    [Benchmark]
    [BenchmarkCategory("Foreach")]
    public void ForeachFrozenSet()
    {
        foreach (var item in _frozenSet)
        {
            _ = item;
        }
    }

    [Benchmark]
    [BenchmarkCategory("Foreach")]
    public void ForeachHashSet()
    {
        foreach (var item in _hashSet)
        {
            _ = item;
        }
    }

    [Benchmark]
    [BenchmarkCategory("Foreach")]
    public void ForeachImmutableDictionary()
    {
        foreach (var item in _immutableDictionary)
        {
            _ = item.Key;
        }
    }

    [Benchmark]
    [BenchmarkCategory("Foreach")]
    public void ForeachImmutableHashSet()
    {
        foreach (var item in _immutableHashSet)
        {
            _ = item;
        }
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Foreach")]
    public void ForeachList()
    {
        foreach (var item in _list)
        {
            _ = item;
        }
    }

    #endregion

}
