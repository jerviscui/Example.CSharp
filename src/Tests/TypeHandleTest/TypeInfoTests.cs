using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TypeHandleTest;

[SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
public class TypeInfoTests
{

    #region Methods

    public void Type_GetProperties_Test()
    {
        foreach (var pi in typeof(B).GetProperties())
        {
            Console.WriteLine(pi.Name);
        }

        //Prop2
        //Prop1
    }

    public void Type_GetRuntimeProperties_Test()
    {
        foreach (var pi in typeof(B).GetRuntimeProperties())
        {
            Console.WriteLine(pi.Name);
        }

        //Prop2
        //PrivateProp2
        //Prop1
    }

    public void TypeInfo_DeclaredProperties_Test()
    {
        foreach (var pi in typeof(B).GetTypeInfo().DeclaredProperties)
        {
            Console.WriteLine(pi.Name);
        }

        //Prop2
        //PrivateProp2
    }

    #endregion

    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
    private class A
    {

        #region Properties

        private int PrivateProp1 { get; set; }

        public int Prop1 { get; set; }

        #endregion
    }

    [SuppressMessage("CodeQuality", "IDE0051:删除未使用的私有成员", Justification = "<挂起>")]
    private sealed class B : A
    {

        #region Properties

        private int PrivateProp2 { get; set; }

        public int Prop2 { get; set; }

        #endregion
    }
}
