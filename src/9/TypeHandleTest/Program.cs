using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace TypeHandleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            new Test().GetAttributes();
            Console.WriteLine();
            new Test().Handle();
            Console.WriteLine();
            new Test().GenericHandle();
            Console.WriteLine();
            new Test().ReferenceGenericHandle();


            Console.ReadKey();
        }
    }

    public class HandleTest
    {
        public void RuntimeHandleAndType()
        {
            var a = 1 == 1;

            Show("Before doing anything");

            //从MSCorlib.dll中地所有方法构建methodInfos 对象缓存
            List<MethodBase>? methodInfos = new List<MethodBase>();
            foreach (Type t in typeof(object).Assembly.GetExportedTypes())
            {
                if (t.IsGenericType) continue;
                MethodInfo[] mbs = t.GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.FlattenHierarchy);
                methodInfos.AddRange(mbs);
            }

            //显示当绑定所有方法之后，方法的个数和堆的大小
            Console.WriteLine("# of Methods={0:###,###}", methodInfos.Count);
            Show("After building cache of MethodInfo objects");

            //为所有MethodInfo对象构建RuntimeMethodHandle缓存
            List<RuntimeMethodHandle>? methodHandles;
            methodHandles = methodInfos.ConvertAll(m => m.MethodHandle);
            Show("Holding MethodInfo and RuntimeMethodHandle");
            GC.KeepAlive(methodHandles);//阻止缓存被过早垃圾回收

            methodInfos = null;//现在允许缓存垃圾回收
            Show("After freeing MethodInfo objects");

            methodInfos = methodHandles.ConvertAll(r => MethodBase.GetMethodFromHandle(r)!);
            Show("Size of heap after re-creating methodinfo objects");
            GC.KeepAlive(methodHandles);//阻止缓存被过早垃圾回收
            GC.KeepAlive(methodInfos);//阻止缓存被过早垃圾回收

            methodInfos = null;//现在允许缓存垃圾回收
            methodHandles = null;//现在允许缓存垃圾回收
            Show("after freeing MethodInfo and MethodHandle objects");

            void Show(string s)
            {

                GC.Collect();
            }
        }
    }
}
