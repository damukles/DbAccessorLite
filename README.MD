# DbAccessorLite

A small, experimental wrapper for System.Data.SqlClient for easier queries.

## Projects

### DbAccessorLite.Nano

- Raw DataTable return value
- DataTable Extensions for easier access to values

Example:

```csharp
var db = new DbAccessorNano(connectionString);
DataTable table = db.Query("select * from Persons");
IEnumerable<Row> rows = table.GetRows(new string[] { "Id", "Name" });
```

### DbAccessorLite.Micro

- Generic Class mapping
- Column to Property Mapping

Example:

```csharp
var db = new DbAccessorMicro(connectionString);
IEnumerable<Person> persons = await db.QueryAsync<Person>("select * from Persons");
```

### Plans

- [x] Async Support
- [x] Decide for a mode => Nano and Micro
- [x] Support for MSSQL and MySql => DbConnection with DbConnectionFactory
- [ ] Escape user input (with sql variables?)
- [ ] Migrations
- [ ] Joins, Object Relations
