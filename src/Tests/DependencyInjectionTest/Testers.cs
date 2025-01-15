namespace DependencyInjectionTest;

internal interface ITester
{
    public abstract string Name { get; }
}

internal sealed class SimpleTester : ITester
{
    /// <inheritdoc />
    public string Name => "SimpleTester";
}

internal sealed class FirstTester : ITester
{
    /// <inheritdoc />
    public string Name => "FirstTester";
}

internal sealed class SecondTester : ITester
{
    /// <inheritdoc />
    public string Name => "SecondTester";
}
