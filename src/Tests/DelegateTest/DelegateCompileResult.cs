using System;
using System.Diagnostics.CodeAnalysis;

namespace DelegateTest
{
    /// <summary>
    /// 在方法中声明 Delegate 变量，编译为使用 Delegate 静态实例
    /// </summary>
    public class Result1
    {
        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        public void DelegateInMethod()
        {
#pragma warning disable IDE0039 // 使用本地函数
            Func<int, int> func = i => i + 1;
#pragma warning restore IDE0039 // 使用本地函数

            func.Invoke(1);
        }
    }

    /// <summary>
    /// 在类中声明  Delegate 变量，在初始化器创建 Delegate 静态实例
    /// </summary>
    public class Result2
    {
        private readonly Func<int, int> _func = i => i + 1;

        public void DelegateInClass() => _func.Invoke(1);
    }

    /// <summary>
    /// 动态创建 Delegate，对应编译为多个 Delegate 静态实例
    /// </summary>
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class Result3
    {
        private static Func<int, int> _func = null!;

        public void DelegateInit1()
        {
            _func = i => i + 1;
            _func.Invoke(1);
        }

        public void DelegateInit2()
        {
            _func = i => i + 2;
            _func.Invoke(1);
        }
    }

    /// <summary>
    /// 将方法作为 Delegate 使用，不会创建缓存
    /// </summary>
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class Result4
    {
#pragma warning disable IDE0052 // 删除未读的私有成员
        private static readonly Func<int, int> FuncTmp = Func;
#pragma warning restore IDE0052 // 删除未读的私有成员

#pragma warning disable IDE0051 // 删除未使用的私有成员
        private void NewDelegate()
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
            Func<int, int> func = Func;
            func.Invoke(1);
        }

        private static int Func(int i) => i + 1;
    }
}
