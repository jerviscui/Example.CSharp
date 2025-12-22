using Riok.Mapperly.Abstractions;

namespace MapperlyTest;

public class Car
{

    #region Properties

    public string Name { get; set; } = string.Empty;

    public string? NullStr { get; set; }

    public int NumberOfWheels { get; set; }

    #endregion

}

internal sealed record CarDto(string Name, int NumberOfWheels)
{

    #region Properties

    internal int? IntNull { get; init; }

    internal string MergeStr => $"{Name} {NumberOfWheels}";

    internal string? StringNull { get; init; }

    #endregion

}

[Mapper]
internal sealed partial class CarMapper
{

    #region Methods

    [MapperIgnoreSource(nameof(Car.NullStr))]
    [MapperIgnoreTarget(nameof(CarDto.IntNull))]
    internal partial CarDto ToCarDto(Car car);

    #endregion

}

[Mapper]
internal sealed partial class CarNullThrowMapper
{

    #region Methods

    [MapProperty(nameof(Car.NullStr), nameof(CarDto.StringNull))]
    internal partial CarDto ToCarDto(Car car);

    #endregion

}
