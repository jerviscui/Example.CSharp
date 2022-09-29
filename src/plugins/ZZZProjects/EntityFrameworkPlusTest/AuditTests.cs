using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EfCoreTest;
using Z.EntityFramework.Plus;

namespace EntityFrameworkPlusTest;

internal class AuditTests : DbContextTest
{
    public static async Task Audit_Test()
    {
        var dbContext = CreateMsSqlDbContext();

        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.ReferenceHandler = ReferenceHandler.Preserve;

        var sb = new StringBuilder();
        AuditManager.DefaultConfiguration.AutoSavePreAction = (context, audit) =>
        {
            foreach (var entry in audit.Entries)
            {
                sb.AppendLine(JsonSerializer.Serialize(entry, options));
            }

            audit.Entries = new List<AuditEntry>();
            Console.WriteLine(sb.ToString());
        };

        var audit = new Audit { CreatedBy = "AuditTests" };

        var person = await dbContext.Persons.AddAsync(new Person(1000, "test", 3, null));
        await dbContext.SaveChangesAsync(audit);

        person.Entity.SetProp(1, 2);
        await dbContext.SaveChangesAsync(audit);

        //{"$id":"1","auditEntryID":0,"createdBy":"AuditTests","createdDate":"2022-09-30T00:47:44.2882646+08:00","entitySetName":"Persons","entityTypeName":"Person","properties":{"$id":"2","$values":[{"$id":"3","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"Id","relationName":null,"isValueSet":false,"internalPropertyName":"Id","newValueFormatted":"1000","isKey":true,"oldValueFormatted":null},{"$id":"4","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"Decimal","relationName":null,"isValueSet":false,"internalPropertyName":"Decimal","newValueFormatted":"0","isKey":false,"oldValueFormatted":null},{"$id":"5","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"FamilyId","relationName":null,"isValueSet":false,"internalPropertyName":"FamilyId","newValueFormatted":"3","isKey":false,"oldValueFormatted":null},{"$id":"6","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"Long","relationName":null,"isValueSet":false,"internalPropertyName":"Long","newValueFormatted":"0","isKey":false,"oldValueFormatted":null},{"$id":"7","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"Name","relationName":null,"isValueSet":false,"internalPropertyName":"Name","newValueFormatted":"test","isKey":false,"oldValueFormatted":null},{"$id":"8","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"TeacherId","relationName":null,"isValueSet":false,"internalPropertyName":"TeacherId","newValueFormatted":null,"isKey":false,"oldValueFormatted":null}]},"state":0,"stateName":"EntityAdded"}
        //{"$id":"1","auditEntryID":0,"createdBy":"AuditTests","createdDate":"2022-09-30T00:47:45.1147978+08:00","entitySetName":"Persons","entityTypeName":"Person","properties":{"$id":"2","$values":[{"$id":"3","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"Id","relationName":null,"isValueSet":false,"internalPropertyName":"Id","newValueFormatted":"1000","isKey":true,"oldValueFormatted":"1000"},{"$id":"4","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"Decimal","relationName":null,"isValueSet":false,"internalPropertyName":"Decimal","newValueFormatted":"2","isKey":false,"oldValueFormatted":"0"},{"$id":"5","auditEntryPropertyID":0,"auditEntryID":0,"parent":{"$ref":"1"},"propertyName":"Long","relationName":null,"isValueSet":false,"internalPropertyName":"Long","newValueFormatted":"1","isKey":false,"oldValueFormatted":"0"}]},"state":2,"stateName":"EntityModified"}
    }
}
