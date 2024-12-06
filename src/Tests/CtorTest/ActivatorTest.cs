using CommandLine;
using System;

namespace CtorTest;

public class ActivatorTest
{

    #region Constants & Statics

    public static void CreateWithPrivate()
    {
        var myData = Activator.CreateInstance<MyData2>();
        _ = myData.Name;
    }

    public static void CreateWithProtected()
    {
        var myData = Activator.CreateInstance<MyData>();
        _ = myData.Name;
    }

    public static void CreateWithProtectedSuccess()
    {
        var myData = Activator.CreateInstance(typeof(MyData), true).Cast<MyData>();
        _ = myData.Name;
    }

    #endregion

    public class MyData
    {
        protected MyData()
        {
            Name = "protected";
        }

        public MyData(string name)
        {
            Name = name;
        }

        #region Properties

        public string Name { get; private set; }

        #endregion
    }

    public class MyData2
    {
        private MyData2()
        {
            Name = "protected";
        }

        public MyData2(string name)
        {
            Name = name;
        }

        #region Properties

        public string Name { get; private set; }

        #endregion
    }
}
