using System;

namespace EfCoreTest
{
    public class MssqlRowVersion : Entity
    {
        public string Name { get; set; }

        public byte[] RowVersion { get; set; }
    }

    public class MysqlRowVersion : Entity
    {
        public string Name { get; set; }

        public DateTime RowVersion { get; set; }
    }

    public class PgsqlRowVersion : Entity
    {
        public string Name { get; set; }
    }
}
