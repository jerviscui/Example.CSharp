using CommunityToolkit.HighPerformance.Buffers;

namespace MemoryModelTest;

public static class MemoryOwnerTest
{

    #region Constants & Statics

    public static void SliceTest()
    {
        using var buffer = MemoryOwner<int>.Allocate(42);
        if (buffer.Length > 50)
        {
            using var buffer2 = buffer[..50];

            var span = buffer2.Span;
            span[0] = 123;
        }
    }

    #endregion

}
