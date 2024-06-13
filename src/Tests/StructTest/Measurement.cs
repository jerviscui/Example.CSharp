namespace StructTest;

public readonly struct Measurement
{
    public Measurement()
    {
        Value = double.NaN;
        Description = "Undefined";
    }

    #region Properties

    public string Description { get; init; }

    public double Value { get; init; }

    #endregion

    #region Methods

    public override string ToString()
    {
        return $"{Value} ({Description})";
    }

    #endregion

}
