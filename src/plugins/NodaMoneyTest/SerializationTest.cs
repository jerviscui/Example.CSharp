using NodaMoney;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text.Json;

namespace NodaMoneyTest;

public static class SerializationTest
{

    #region Constants & Statics

    public static void Input_Validation_Test()
    {
        var input = new MyInput("Jane Doe", 25, new FastMoneyDto { Amount = 49.95m, });
        var json = JsonSerializer.Serialize(input);
        Console.WriteLine(json);
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

public record MyInput(string Name, int Age, [AmountValidation("1.00000", "100")] FastMoneyDto Price);

public record MyOutput(string Name, int Age, FastMoneyDto Price);

public static class FastMoneyExtensions
{

    #region Constants & Statics

    public static FastMoneyDto ToDto(this FastMoney fastMoney)
    {
        return new FastMoneyDto { Amount = fastMoney.Amount, Currency = fastMoney.Currency.Code };
    }

    #endregion

}

public class FastMoneyDto : IValidatableObject
{

    #region Properties

    public decimal Amount { get; init; }

    public string Currency { get; init; } = CurrencyCode.CNY;

    #endregion

    #region IValidatableObject implementations

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var results = new List<ValidationResult>();

        //validate AmountValidationAttribute
        var context = new ValidationContext(this);
        _ = Validator.TryValidateProperty(Amount, context, results);

        try
        {
            _ = CurrencyInfo.FromCode(Currency);
        }
        catch
        {
            results.Add(new ValidationResult("Invalid currency code.", [nameof(Currency)]));
        }

        return results;
    }

    #endregion

}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class AmountValidationAttribute : ValidationAttribute
{
    [SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "<Pending>")]
    public AmountValidationAttribute(string minimum, string maximum)
    {
        Minimum = decimal.Parse(minimum, CultureInfo.InvariantCulture);
        Maximum = decimal.Parse(maximum, CultureInfo.InvariantCulture);

        ArgumentOutOfRangeException.ThrowIfLessThan(Minimum, FastMoney.MinValue.Amount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(Maximum, FastMoney.MaxValue.Amount);

        if (Minimum.Scale > 4)
        {
            throw new ArgumentException("Scale cannot be greater than 4.", nameof(minimum));
        }
        if (Maximum.Scale > 4)
        {
            throw new ArgumentException("Scale cannot be greater than 4.", nameof(maximum));
        }
    }

    #region Properties

    /// <summary>
    /// Gets the maximum allowed field value.
    /// </summary>
    public decimal Maximum { get; }

    /// <summary>
    /// Specifies whether validation should fail for values that are equal to System.ComponentModel.DataAnnotations.RangeAttribute.Maximum.
    /// </summary>
    public bool MaximumIsExclusive { get; set; }

    /// <summary>
    /// Gets the minimum allowed field value.
    /// </summary>
    public decimal Minimum { get; }

    /// <summary>
    /// Specifies whether validation should fail for values that are equal to System.ComponentModel.DataAnnotations.RangeAttribute.Minimum.
    /// </summary>
    public bool MinimumIsExclusive { get; set; }

    #endregion

}
