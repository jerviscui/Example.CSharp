namespace EfCoreTest;

public class MssqlRowVersion : Entity
{

    #region Properties

    public string Name { get; set; } = null!;

    public byte[] RowVersion { get; set; } = null!;

    #endregion

}

public class MysqlRowVersion : Entity
{

    #region Properties

    public string Name { get; set; } = null!;

    public DateTime RowVersion { get; set; }

    #endregion

}

public class PgsqlRowVersion : Entity
{

    #region Properties

    public string Name { get; set; } = null!;

    public uint Version { get; set; }

    #endregion

}
