using Riok.Mapperly.Abstractions;

namespace MapperlyTest;

public class Dog
{

    #region Properties

    public string Name { get; set; } = string.Empty;

    public string? NullStr { get; set; }

    public int NumberOfWheels { get; set; }

    #endregion

}

internal sealed record DogDto
{
    //PreferParameterlessConstructors = true use this constructor
    internal DogDto()
    {
        Name = string.Empty;
        NumberOfWheels = 1;
    }

    [MapperConstructor]
    internal DogDto(int numberOfWheels)
    {
        Name = string.Empty;
        NumberOfWheels = numberOfWheels;
        StringNull = "test1";
    }

    // PreferParameterlessConstructors = false use this constructor
    internal DogDto(string name, int numberOfWheels)
    {
        Name = name;
        NumberOfWheels = numberOfWheels;
        StringNull = "test2";
    }

    #region Properties

    internal int? IntNull { get; set; }

    internal string MergeStr => $"{Name} {NumberOfWheels}";

    internal string Name { get; init; }

    internal int NumberOfWheels { get; init; }

    internal string? StringNull { get; set; }

    #endregion

}

[Mapper]
internal sealed partial class DogMapper
{

    #region Methods

    [MapProperty(nameof(Dog.NullStr), nameof(DogDto.StringNull))]
    internal partial DogDto ToDogDto(Dog dog);

    internal partial ICollection<DogDto> ToDogDtos(ICollection<Dog> dogs);

    #endregion

}

[Mapper]
internal static partial class DogProjectToMapper
{

    #region Constants & Statics

    [IncludeMappingConfiguration(nameof(@DogMapper.ToDogDto))]
    private static partial DogDto ToDogDto(Dog dog);

    internal static partial IQueryable<DogDto> ProjectToDogDto(this IQueryable<Dog> query);

    #endregion

}

[Mapper]
internal static partial class DogProjectToWithCustomMapper
{

    #region Constants & Statics

    private static string StringNullSet(string? source)
    {
        return source ?? "default str";
    }

    [MapProperty(nameof(Dog.NullStr), nameof(DogDto.StringNull), Use = nameof(StringNullSet))]
    private static partial DogDto ToDogDto(Dog dog);

    internal static partial IQueryable<DogDto> ProjectToDogDto2(this IQueryable<Dog> query);

    #endregion

}

[Mapper]
internal static partial class DogDtoExtensionsMapper
{

    #region Constants & Statics

    private static string StringNullSet(Dog dog)
    {
        return dog.NullStr ?? "default str";
    }

    [MapPropertyFromSource(nameof(DogDto.StringNull), Use = nameof(StringNullSet))]
    [MapperIgnoreTarget(nameof(DogDto.Name))]
    [MapperIgnoreTarget(nameof(DogDto.NumberOfWheels))]
    internal static partial void FromDog([MappingTarget] this DogDto dogDto, Dog dog);

    #endregion

}
