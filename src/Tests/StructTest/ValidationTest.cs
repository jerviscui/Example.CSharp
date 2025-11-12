using System.ComponentModel.DataAnnotations;

namespace StructTest;

public static class ValidationTest
{

    #region Constants & Statics

    public static void ValidationAllow()
    {
        var product = new Product
        {
            Price = 100_000.00m,
            Price2 = 0.01m,
            Price3 = 1_000_000_000_000_000, // no error, is wrong!
            Price33 = 1_000_000_000_000_000, // no error, is wrong!
            Price4 = 999_999_999_999_999.99981m //  no error
        };
        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(product, context, results, true);

        if (!isValid)
        {
            foreach (var error in results)
            {
                Console.WriteLine($"{string.Join(',', error.MemberNames)} : {error.ErrorMessage}");
            }
        }
    }

    public static void ValidationError()
    {
        var product = new Product
        {
            Price = -1.00m,
            Price2 = 0.001m,
            Price3 = 999_999_999_999_999.99991m, // no error, is wrong!
            Price33 = 999_999_999_999_999.99991, // no error, is wrong!
            Price4 = 999_999_999_999_999.99991m // error
        };
        var context = new ValidationContext(product);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(product, context, results, true);

        if (!isValid)
        {
            foreach (var error in results)
            {
                Console.WriteLine($"{string.Join(',', error.MemberNames)} : {error.ErrorMessage}");
            }
        }
    }

    #endregion

    public class Product
    {

        #region Properties

        [Range(0.01, 100_000.00, ErrorMessage = "价格必须在0.01到100,000.00之间")]
        public decimal Price { get; set; }

        [Range(typeof(decimal), "0.01", "100000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price2 { get; set; }

        [Range(0.0001, 999_999_999_999_999.9999)] // Range参数只有double，所以不能超过 double 有效位数，between 0.0001 and 1000000000000000
        public decimal Price3 { get; set; }

        [Range(typeof(double), 0.0001, 999_999_999_999_999.9999)] // 
        public double Price33 { get; set; }

        [Range(typeof(decimal), "0.0001", "999999999999999.9999")] // decimal Range条件需要使用string参数，避免double精度问题
        public decimal Price4 { get; set; }

        #endregion
    }
}
