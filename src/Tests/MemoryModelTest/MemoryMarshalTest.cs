using System;
using System.Runtime.InteropServices;

namespace MemoryModelTest;

public static class MemoryMarshalTest
{

    #region Constants & Statics

    public static unsafe void GetReferenceNull_WithEmptySpanTest()
    {
#pragma warning disable CA1825
        var array = new byte[0];
#pragma warning restore CA1825
        var a = new Span<byte>(array);

        ref var r = ref MemoryMarshal.GetReference(a);
        //r is 0

        fixed (byte* pbData = &MemoryMarshal.GetReference(a))
        {
            //pbData is not null, 0x000002191c40ee30
        }
    }

    public static unsafe void GetReferenceNullTest()
    {
        var a = new Span<byte>(null);

        ref var r = ref MemoryMarshal.GetReference(a);
        //'r' threw an exception of type 'System.NullReferenceException'

        fixed (byte* pbData = &MemoryMarshal.GetReference(a))
        {
            //pbData is 0x0000000000000000, is null
        }
    }

    #endregion

}
