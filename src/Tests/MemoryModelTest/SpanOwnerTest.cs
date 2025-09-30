using CommunityToolkit.HighPerformance.Buffers;

namespace MemoryModelTest;

public static class SpanOwnerTest
{

    #region Constants & Statics

    public static void Test()
    {
        using var buffer = SpanOwner<int>.Allocate(100);

        var span = buffer.Span;
    }

    #endregion

}
