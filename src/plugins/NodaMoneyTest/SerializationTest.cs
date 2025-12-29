using NodaMoney;
using System.Text.Json;

namespace NodaMoneyTest;

internal static class SerializationTest
{

    #region Constants & Statics

    public static void Serialize_Test()
    {
        var dto = new MyDto("John Doe", 30, new FastMoney(99.99m, "USD"));
        var json = JsonSerializer.Serialize(dto);

        Console.WriteLine(json);

        var deserializedDto = JsonSerializer.Deserialize<MyDto>(json);
    }

    #endregion

}

public record MyDto(string Name, int Age, FastMoney Price);
