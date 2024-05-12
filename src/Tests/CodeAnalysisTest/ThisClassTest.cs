namespace CodeAnalysisTest;

public class ThisClassTest
{

    private int capacity;

    public ThisClassTest()
    {
        // IDE0003 dotnet_style_qualification_for_field = false
        this.capacity = 0;

        // IDE0003 dotnet_style_qualification_for_property = false
        this.ID = 0;

        // IDE0003 dotnet_style_qualification_for_method = false
        this.Display();

        // IDE0003 dotnet_style_qualification_for_event = false
        this.MyEvent += (sender, e) => { };

        Test();
    }
    public int ID { get; set; }

    private void Display()
    {
        throw new NotImplementedException();
    }

    public event EventHandler MyEvent;

    private static bool Test()
    {
        // ide0007 csharp_style_var_for_built_in_types = true
        int x = 5;
        int y = x;

        // ide0007 csharp_style_var_when_type_is_apparent = true
        MyClass obj = new MyClass();
        var yy = obj.Age;

        // ide0007 csharp_style_var_elsewhere = true
        bool f = Random.Shared.Next() > 1;
        var yyy = f;

        return true;
    }
}
