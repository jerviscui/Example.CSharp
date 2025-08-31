using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace MemoryModelTest;

public class BitConverterEx
{

    #region Constants & Statics

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern string FastAllocateString(int length);

    public static string ToString(ReadOnlySpan<byte> bytes)
    {
        if (bytes.IsEmpty)
        {
            return string.Empty;
        }

        // (int.MaxValue / 3) == 715,827,882 Bytes == 699 MB
        var sb = new StringBuilder((bytes.Length * 3) - 1);
        sb.Append(bytes[0].ToString("X2"));
        for (var i = 1; i < bytes.Length; i++)
        {
            sb.Append('-');
            sb.Append(bytes[i].ToString("X2"));
        }
        return sb.ToString();
    }

    #endregion

}
