using NodaMoney;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace NodaMoneyTest;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class FastMoneyRangeAttribute : ValidationAttribute
{
    [SuppressMessage("Design", "CA1019:Define accessors for attribute arguments", Justification = "<Pending>")]
    public FastMoneyRangeAttribute(string minimum, string maximum)
    {
        Minimum = decimal.Parse(minimum, CultureInfo.InvariantCulture);
        Maximum = decimal.Parse(maximum, CultureInfo.InvariantCulture);
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

    #region Methods

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(Minimum, FastMoney.MinValue.Amount);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(Maximum, FastMoney.MaxValue.Amount);

        if (Minimum.Scale > 4)
        {
            throw new ArgumentException("Minimum scale cannot be greater than 4.");
        }
        if (Maximum.Scale > 4)
        {
            throw new ArgumentException("Maximum scale cannot be greater than 4.");
        }

        if (value is FastMoneyDto dto)
        {
            if (dto.Amount < Minimum || (MinimumIsExclusive && dto.Amount == Minimum))
            {
                return new ValidationResult(
                    $"The field {validationContext.MemberName}.{nameof(FastMoneyDto.Amount)} must be between {Minimum} and {Maximum}.",
                    [nameof(FastMoneyDto.Amount)]);
            }
            if (dto.Amount > Maximum || (MaximumIsExclusive && dto.Amount == Maximum))
            {
                return new ValidationResult(
                    $"The field {validationContext.MemberName}.{nameof(FastMoneyDto.Amount)} must be between {Minimum} and {Maximum}.",
                    [nameof(FastMoneyDto.Amount)]);
            }

            if (!CurrencyCode.SupportedCurrency.ContainsKey(dto.Currency))
            {
                return new ValidationResult("Invalid currency code.", [nameof(Currency)]);
            }
        }

        return ValidationResult.Success;
    }

    #endregion

}
