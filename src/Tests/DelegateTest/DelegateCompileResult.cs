using System;

namespace DelegateTest
{
    /// <summary>
    /// 在方法中声明 Delegate 变量，编译为使用 Delegate 静态实例
    /// </summary>
    public class Result1
    {
        public void DelegateInMethod()
        {
            Func<int, int> func = i => i + 1;

            func.Invoke(1);
        }
    }

    /// <summary>
    /// 在类中声明  Delegate 变量，在初始化器创建 Delegate 静态实例
    /// </summary>
    public class Result2
    {
        private Func<int, int> _func = i => i + 1;

        public void DelegateInClass()
        {
            _func.Invoke(1);
        }
    }

    /// <summary>
    /// 动态创建 Delegate，对应编译为多个 Delegate 静态实例
    /// </summary>
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
    public class Result4
    {
        private static Func<int, int> _funcTmp = Func;

        private void NewDelegate()
        {
            Func<int, int> func = Func;
            func.Invoke(1);
        }

        private static int Func(int i)
        {
            return i + 1;
        }
    }
}