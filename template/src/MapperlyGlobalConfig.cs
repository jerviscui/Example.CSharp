using Riok.Mapperly.Abstractions;

[assembly: MapperDefaults(
    AllowNullPropertyAssignment = true,
    ThrowOnPropertyMappingNullMismatch = true,
    ThrowOnMappingNullMismatch = true,
    RequiredMappingStrategy = RequiredMappingStrategy.None,
    AutoUserMappings = false,
    IgnoreObsoleteMembersStrategy = IgnoreObsoleteMembersStrategy.None,
    PreferParameterlessConstructors = true)]
