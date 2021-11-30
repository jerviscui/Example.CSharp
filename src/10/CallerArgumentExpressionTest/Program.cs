//"123".LessThan(1); //System.ArgumentException: "123" must less than 1

var myClass = new MyClass { Name = "aaa" };
myClass.Name.LessThan(1); //Unhandled exception. System.ArgumentException: myClass.Name must less than 1

internal class MyClass
{
    public string Name { get; set; }
}
