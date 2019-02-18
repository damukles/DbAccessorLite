using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DbAccessorLite.Micro;

namespace DbAccessorLite.Nano.TestApp
{
    class Program
    {
        static void Main(string[] args) => MainAsync(args).GetAwaiter().GetResult();
        static async Task MainAsync(string[] args)
        {
            // var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Test;Trusted_Connection=True;";
            var connectionStringMySql = "Server=localhost;Database=Test;Uid=root;Pwd=123456;";

            // var db = new DbAccessorNano(connectionString);
            var db = new DbAccessorNano(connectionStringMySql, (cs) => new MySql.Data.MySqlClient.MySqlConnection(cs));
            DataTable table = db.Query("select * from Persons");
            IEnumerable<string> names = table.GetColumnValues("Name");

            Console.WriteLine("=== All fields in Name Column ===");
            Console.WriteLine(String.Join(", ", names));

            IEnumerable<Row> rows = table.GetRows(new string[] { "Id", "Name" });
            Console.WriteLine("=== Some Fields of all Rows ===");
            foreach (var row in rows)
            {
                var idField = row["Id"];
                var nameField = row["Name"];
                Console.WriteLine(String.Join(", ", new string[] { idField, nameField }));
            }


            // var db2 = new DbAccessorMicro(connectionString);
            var db2 = new DbAccessorMicro(connectionStringMySql, (cs) => new MySql.Data.MySqlClient.MySqlConnection(cs));
            Console.WriteLine("=== async Person insert ===");
            var insertedCount = await db2.ExecuteAsync("insert into Persons (Name, City) values ('Kurt', 'Züri')");
            Console.WriteLine($"Inserted row count: {insertedCount}");

            var persons = await db2.QueryAsync<Person>("select * from Persons");
            Console.WriteLine("=== async Person class Mappings ===");
            foreach (var person in persons)
            {
                Console.WriteLine(String.Join(", ", new string[] { person.Id.ToString(), person.Name, person.Address }));
            }

            var deletedCount = await db.ExecuteAsync("delete from Persons where Name='Kurt'");
        }
    }
}
