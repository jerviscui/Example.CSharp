using BenchmarkDotNet.Attributes;
using System.Diagnostics.CodeAnalysis;

namespace StructTest;

public class StandardClass
{

    #region Properties

    public int StandardProperty { get; set; }

    #endregion

    #region Methods

    public int GetStandardProperty()
    {
        return StandardProperty;
    }

    #endregion

}

public sealed class SealedClass
{

    #region Properties

    public int SealedProperty { get; set; }

    #endregion

    #region Methods

    public int GetSealedProperty()
    {
        return SealedProperty;
    }

    #endregion

}

public abstract class AbstractBaseClass
{

    #region Methods

    public virtual int GetBaseProperty()
    {
        return 0;
    }

    #endregion

}

public class DerivedClass : AbstractBaseClass
{

    #region Properties

    public int DerivedProperty { get; set; }

    #endregion

    #region Methods

    public override int GetBaseProperty()
    {
        return DerivedProperty;
    }

    #endregion

}

public class ExtendedStandardClass : StandardClass
{

    #region Properties

    public int AdditionalProperty { get; set; }

    #endregion

    #region Methods

    public int GetAdditionalProperty()
    {
        return AdditionalProperty;
    }

    #endregion

}

[SuppressMessage("Naming", "CA1707:Identifiers should not contain underscores", Justification = "<Pending>")]
public class ClassBenchmark
{
    private readonly DerivedClass _derivedClassInstance = new() { DerivedProperty = 42 };
    private readonly ExtendedStandardClass _extendedClassInstance = new()
    {
        StandardProperty = 42,
        AdditionalProperty = 84
    };
    private readonly SealedClass _sealedClassInstance = new() { SealedProperty = 42 };
    private readonly StandardClass _standardClassInstance = new() { StandardProperty = 42 };

    #region Methods

    [Benchmark]
    public int DerivedClass_Property()
    {
        return _derivedClassInstance.GetBaseProperty();
    }

    [Benchmark]
    public int ExtendedClass_AdditionalProperty()
    {
        return _extendedClassInstance.GetAdditionalProperty();
    }

    [Benchmark]
    public int ExtendedClass_StandardProperty()
    {
        return _extendedClassInstance.GetStandardProperty();
    }

    [Benchmark]
    public int SealedClass_Property()
    {
        return _sealedClassInstance.GetSealedProperty();
    }

    [Benchmark(Baseline = true)]
    public int StandardClass_Property()
    {
        return _standardClassInstance.GetStandardProperty();
    }

    #endregion

}
