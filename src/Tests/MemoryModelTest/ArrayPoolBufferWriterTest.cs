using CommunityToolkit.HighPerformance.Buffers;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MemoryModelTest;

internal static class ArrayPoolBufferWriterTest
{

    #region Constants & Statics

    public static void Test()
    {
        using var writer = new ArrayPoolBufferWriter<byte>();

        // 获取可写入的缓冲区
        var buffer = writer.GetSpan();

        // 写入数据
        var data = new byte[] { 1, 2, 3, 4, 5 };
        data.CopyTo(buffer);
        // 标记实际写入的长度
        writer.Advance(data.Length);

        // 可以反复写入
        var memory = writer.GetMemory(32); // 申请新的空间
        data.CopyTo(memory);
        writer.Advance(data.Length);

        // 获取写入后的所有数据，也可以用 WrittenMemory / WrittenCount
        //Console.WriteLine(BitConverterEx.ToString(writer.WrittenSpan));
        unsafe
        {
            var ptr2 = new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(writer.WrittenSpan)));
            Console.WriteLine(ptr2);//2106795701488
        }
        if (MemoryMarshal.TryGetArray(writer.WrittenMemory, out var arraySegment))
        {
            Debug.Assert(arraySegment.Array is not null, "arraySegment.Array != null");
            var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(arraySegment.Array, 0);
            Console.WriteLine(ptr);//2106795701488 same with writer.WrittenSpan

            Console.WriteLine(BitConverter.ToString(arraySegment.Array, arraySegment.Offset, arraySegment.Count));
            //01-02-03-04-05-01-02-03-04-05
        }
    }

    #endregion

}
