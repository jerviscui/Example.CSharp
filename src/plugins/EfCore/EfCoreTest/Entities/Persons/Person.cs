namespace EfCoreTest;

public class Person : Entity
{
    protected Person()
    {
    }

    public Person(long id, string name, long familyId, long? teacherId = null)
    {
        Id = id;
        TeacherId = teacherId;
        FamilyId = familyId;

        ChangeName(name);
        SetProp(0, 0);
    }

    public Person(long id, string name, long familyId, long teacherId, long l = 11, decimal d = 22.22m)
        : this(id, name, familyId, (long?)teacherId)
    {
        SetProp(l, d);
    }

    #region Properties

    public decimal Decimal { get; private set; }

    public long FamilyId { get; protected set; }

    public long Long { get; private set; }

    public string Name { get; set; } = null!;

    public long? TeacherId { get; protected set; }

    #endregion

    #region Methods

    public Person ChangeName(string name)
    {
        Name = name;

        return this;
    }

    public void SetProp(long l, decimal d)
    {
        Long = l;
        Decimal = d;
    }

    #endregion

}
