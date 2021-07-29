using Mapster;
using System;
using System.Reflection;

namespace MapsterTest
{
    public class CodeGenerationRegister : ICodeGenerationRegister
    {
        public void Register(CodeGenerationConfig config)
        {
            //config.AdaptTo("[name]Dto")
            //    .ForType<Student>(builder => 
            //        builder.Settings[nameof(Student.FirstMidName)].TargetPropertyType = typeof(string?));

            //config.AdaptFrom("[name]Merge")
            //    .ForType<Student>()
            //    .IgnoreNullValues(true);

            //config.AdaptTo("[name]Dto").ForType<Enrollment>().MapToConstructor(true);

            //config.AdaptFrom("[name]Dto").ForType<Enrollment>();

            config.AdaptTo("[name]Dto", MapType.Map | MapType.MapToTarget | MapType.Projection)
                .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "MapsterTest.Domains")
                .MapToConstructor(true)
                .ShallowCopyForSameType(true)//浅拷贝
                .ExcludeTypes(o => o.IsEnum || Nullable.GetUnderlyingType(o)?.IsEnum == true)
                .MaxDepth(3)//嵌套深度
                .PreserveReference(true)//缓存 dto 对象
                                        //.ForType<Student>(builder => builder.IsNullableReference(o => o.FirstMidName))
                ;

            config.GenerateMapper("[name]Mapper")
                .ForAllTypesInNamespace(Assembly.GetExecutingAssembly(), "MapsterTest.Domains")
                ;
        }
    }
}
