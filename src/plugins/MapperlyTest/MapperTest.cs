namespace MapperlyTest;

public static class MapperTest
{

    #region Constants & Statics

    public static void CarMapper_Test()
    {
        var mapper = new CarMapper();

        var car = new Car { Name = "Fiat", NumberOfWheels = 4 };
        var dto = mapper.ToCarDto(car);
        Console.WriteLine($"Car: {dto.Name}, Wheels: {dto.NumberOfWheels}");
    }

    public static void CarNullThrowMapper_Test()
    {
        var mapper = new CarNullThrowMapper();

        var car = new Car { Name = "Fiat", NumberOfWheels = 4, NullStr = "aaa" };
        var dto = mapper.ToCarDto(car);
        Console.WriteLine($"Car: {dto.Name}, Wheels: {dto.NumberOfWheels}");
        Console.WriteLine($"StringNull: {dto.StringNull}");
    }

    public static void Ctro_Test()
    {
        var mapper = new DogMapper();

        var dog = new Dog { Name = "Fiat", NumberOfWheels = 4, NullStr = "aaa" };
        var dto = mapper.ToDogDto(dog);
        Console.WriteLine($"{dto.Name}, Wheels: {dto.NumberOfWheels}");
        Console.WriteLine($"StringNull: {dto.StringNull}");
    }

    public static void ExistingTarget_Test()
    {
        var dog = new Dog { Name = "Fiat", NumberOfWheels = 4, NullStr = "aaa" };
        var dto = new DogDto { Name = "OldName", NumberOfWheels = 99, StringNull = "OldString" };
        dto.FromDog(dog);

        Console.WriteLine($"{dto.Name}, Wheels: {dto.NumberOfWheels}");
        Console.WriteLine($"StringNull: {dto.StringNull}");
    }

    public static void List_Test()
    {
        var mapper = new DogMapper();

        var dogs = new List<Dog>
        {
            new() { Name = "Buddy", NumberOfWheels = 4, NullStr = "aaa" },
            new() { Name = "Max", NumberOfWheels = 3, NullStr = "bbb" }
        };

        var list = mapper.ToDogDtos(dogs);

        foreach (var dto in list)
        {
            Console.WriteLine($"{dto.Name}, Wheels: {dto.NumberOfWheels}");
            Console.WriteLine($"StringNull: {dto.StringNull}");
        }
    }

    public static void ProjectTo_Test()
    {
        var dogs = new List<Dog>
        {
            new() { Name = "Buddy", NumberOfWheels = 4, NullStr = "aaa" },
            new() { Name = "Max", NumberOfWheels = 3, NullStr = "bbb" }
        }.AsQueryable();

        var list = dogs.ProjectToDogDto().ToList();

        list.ForEach(
            static dto =>
            {
                Console.WriteLine($"{dto.Name}, Wheels: {dto.NumberOfWheels}");
                Console.WriteLine($"StringNull: {dto.StringNull}");
            });
    }

    #endregion

}
