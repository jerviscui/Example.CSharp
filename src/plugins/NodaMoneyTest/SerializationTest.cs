using NodaMoney;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace NodaMoneyTest;

public static class SerializationTest
{

    #region Constants & Statics

    public static void Input_Validation_ModelState_Test()
    {
        var input = new MyInput("Jane Doe", 25, new FastMoneyDto { Amount = 101m, });
        var context = new ValidationContext(input);
        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(input, context, results, true);

        Console.WriteLine($"MyInput Over Max IsValid: {isValid}");
        foreach (var result in results)
        {
            Console.WriteLine($"Error: {result.ErrorMessage}");
        }

        var input2 = new MyInput("Jane Doe", 25, new FastMoneyDto { Amount = 1m, Currency = "abc" });
        context = new ValidationContext(input2);
        results = [];
        isValid = Validator.TryValidateObject(input2, context, results, true);

        Console.WriteLine($"MyInput Currency IsValid: {isValid}");
        foreach (var result in results)
        {
            Console.WriteLine($"Error: {result.ErrorMessage}");
        }
    }

    public static void Output_Serialization_Test()
    {
        var money = new FastMoney(99.99m);
        var dto = money.ToDto();

        var output = new MyOutput("Alice Smith", 28, dto);
        var json = JsonSerializer.Serialize(output);
        Console.WriteLine(json);
    }

    public static void Serialize_Test()
    {
        var dto = new MyDto("John Doe", 30, new FastMoney(99.99m, "USD"));
        var json = JsonSerializer.Serialize(dto);

        Console.WriteLine(json);
        _ = JsonSerializer.Deserialize<MyDto>(json);
    }

    #endregion

}

public record MyDto(string Name, int Age, FastMoney Price);

public record MyInput(string Name, int Age, [property: FastMoneyRange("1.0000", "100")] FastMoneyDto Price);

public record MyOutput(string Name, int Age, FastMoneyDto Price);
