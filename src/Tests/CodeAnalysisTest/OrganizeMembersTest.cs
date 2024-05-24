namespace CodeAnalysisTest;

public class OrganizeMembersTest
{
    // IDE0001 dotnet_style_predefined_type_for_locals_parameters_members = true
    private int _id;

    // IDE0001 dotnet_style_predefined_type_for_locals_parameters_members = true
    public int ID
    {
        get => _id;
        set => _id = value;
    }

    // IDE0001 dotnet_style_predefined_type_for_locals_parameters_members = true
    public void Display()
    {
        throw new NotImplementedException();
    }

    // IDE0001 dotnet_style_predefined_type_for_locals_parameters_members = true
    public event EventHandler MyEvent;

    // IDE0001 dotnet_style_predefined_type_for_locals_parameters_members = true
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
