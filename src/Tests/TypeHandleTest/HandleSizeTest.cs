using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace TypeHandleTest
{
    public class HandleSizeTest
    {
        public void RuntimeHandleAndType()
        {
            //todo cuizj: 没有发现 MethodInfo 和 RuntimeMethodHandle 使用空间上的明显差距

            Show("Before doing anything");

            //从MSCorlib.dll中地所有方法构建methodInfos 对象缓存
            List<MethodBase>? methodInfos = new List<MethodBase>();
            foreach (Type t in typeof(object).Assembly.GetExportedTypes())
            {
                if (t.IsGenericType) continue;
                MethodInfo[] mbs = t.GetMethods(BindingFlags.Instance | BindingFlags.Static |
                                                BindingFlags.Public | BindingFlags.NonPublic |
                                                BindingFlags.FlattenHierarchy)
                    .Where(o => o.DeclaringType is not null && !o.DeclaringType.IsGenericType).ToArray();
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
            methodInfos.Clear();
            methodInfos = null;//现在允许缓存垃圾回收
            Show("After freeing MethodInfo objects");

            methodInfos = methodHandles.ConvertAll(r => MethodBase.GetMethodFromHandle(r)!);
            Show("Size of heap after re-creating methodinfo objects");
            GC.KeepAlive(methodHandles);//阻止缓存被过早垃圾回收
            GC.KeepAlive(methodInfos);//阻止缓存被过早垃圾回收

            methodInfos.Clear();
            methodInfos = null;//现在允许缓存垃圾回收
            methodHandles.Clear();
            methodHandles = null;//现在允许缓存垃圾回收
            Show("after freeing MethodInfo and MethodHandle objects");

            void Show(string s)
            {
                Thread.Sleep(100);
                GC.Collect();
                Thread.Sleep(100);
                Console.WriteLine($"Heap Size = {GC.GetTotalMemory(false),11:C0}b - {s}");
            }
        }
    }
}