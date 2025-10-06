using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace TypeHandleTest;

public class CallvirtBenchmark
{
    private Derived? _derived;
    private SealedDerived? _sealedObj;

    #region Methods

    [GlobalSetup]
    public void Init()
    {
        _derived = new Derived();
        _sealedObj = new SealedDerived();
    }

    [Benchmark]
    public void NonVirtualMethod()
    {
        _derived!.NonVirtualMethod();
    }

    [Benchmark]
    public void SealedVirtualMethod()
    {
        _sealedObj!.VirtualMethod();
    }

    [Benchmark(Baseline = true)]
    public void StaticMethod()
    {
        Base.StaticMethod();
    }

    [Benchmark]
    public void VirtualMethod()
    {
        _derived!.VirtualMethod();
    }

    #endregion

}

public class Base
{

    #region Constants & Statics

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void StaticMethod()
    {
        //for test
    }

    #endregion

    #region Methods

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void NonVirtualMethod()
    {
        //for test
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public virtual void VirtualMethod()
    {
        //for test
    }

    #endregion

}

public class Derived : Base
{

    #region Methods

    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void VirtualMethod()
    {
        //for test
    }

    #endregion

}

public sealed class SealedDerived : Base
{

    #region Methods

    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void VirtualMethod()
    {
        //for test
    }

    #endregion

}
