namespace DependencyInjectionTest;

internal interface ITester
{
    public abstract string Name { get; }
}

internal class SimpleTester : ITester
{
    /// <inheritdoc />
    public string Name => "SimpleTester";
}

internal class FirstTester : ITester
{
    /// <inheritdoc />
    public string Name => "FirstTester";
}

internal class SecondTester : ITester
{
    /// <inheritdoc />
    public string Name => "SecondTester";
}
