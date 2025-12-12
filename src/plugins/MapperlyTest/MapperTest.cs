namespace MapperlyTest;

public static class MapperTest
{

    #region Constants & Statics

    public static void Test()
    {
        var mapper = new CarMapper();
        var car = new Car { Name = "Fiat", NumberOfWheels = 4 };
        var dto = mapper.CarToCarDto(car);
        Console.WriteLine($"Car: {dto.Name}, Wheels: {dto.NumberOfWheels}");
    }

    #endregion

}
