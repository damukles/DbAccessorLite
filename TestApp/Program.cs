using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DbAccessorLite.Micro;
using DbAccessorLite.Migrations;

namespace DbAccessorLite.Nano.TestApp
{
    class Program
    {
        static readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Test;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            Migrate();

            TestNano();
            TestMicro().GetAwaiter().GetResult();

            TearDown();
        }

        static void Migrate()
        {
            new SqlServerMigrator(_connectionString)
                .AddTable("Persons", table =>
                {
                    table.WithPrimaryKeyColumn("Id", typeof(int))
                        .AddColumn("Name", typeof(string))
                        .AddColumn("Address", typeof(string))
                        .AddColumn("City", typeof(string));
                })
                .AddScript(
                    @"INSERT INTO [dbo].[Persons]
                        ([Name]
                        ,[Address]
                        ,[City])
                    VALUES
                        ('Daniel'
                        ,'Bluemeweg 18'
                        ,'Hinterpfupfikon'),
                        ('Stefan'
                        ,'Hortensiestross 16'
                        ,'Oberunterhinterhausen'),
                        ('Corina'
                        ,'Rosengasse 17'
                        ,'Nebenflosshusen')"
                )
                .Migrate();
        }

        static void TestNano()
        {

            var db = new DbAccessorNano(_connectionString);
            // var db = new DbAccessorNano(_connectionString, (cs) => new MySql.Data.MySqlClient.MySqlConnection(cs));
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
        }

        static async Task TestMicro()
        {
            var db = new DbAccessorMicro(_connectionString);
            // var db = new DbAccessorMicro(_connectionString, (cs) => new MySql.Data.MySqlClient.MySqlConnection(cs));
            Console.WriteLine("=== async Person insert ===");
            var insertedCount = await db.ExecuteAsync("insert into Persons (Name, City) values ('Kurt', 'Züri')");
            Console.WriteLine($"Inserted row count: {insertedCount}");

            var persons = await db.QueryAsync<Person>("select * from Persons");
            Console.WriteLine("=== async Person class Mappings ===");
            foreach (var person in persons)
            {
                Console.WriteLine(String.Join(", ", new string[] { person.Id.ToString(), person.Name, person.Address }));
            }

            var deletedCount = await db.ExecuteAsync("delete from Persons where Name='Kurt'");
        }

        static void TearDown()
        {
            new SqlServerMigrator(_connectionString)
                .RemoveTable("Persons")
                .Migrate();
        }
    }
}
