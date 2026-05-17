using MapperlyTest;
using Riok.Mapperly.Abstractions;

namespace OtherNamespace;

[Mapper]
public static partial class NestedClassMapper
{

    #region Constants & Statics

    [MapProperty(nameof(Dog.NestedClass.Prop), nameof(DogDto.NestedDto.PropDto))]
    public static partial DogDto.NestedDto ToNestedDto(this Dog.NestedClass nested);

    #endregion

}
