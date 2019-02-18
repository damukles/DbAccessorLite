using System;
using System.Collections.Generic;

namespace DbAccessorLite.Migrations
{
    public class Table : DbArtifact
    {
        private readonly string _name;
        private readonly string _baseScript;

        internal Table(string name)
        {
            _name = name;
            _baseScript = $"IF NOT EXISTS (select * from sys.tables where name='{_name}') CREATE TABLE {_name}";
        }

        public Table WithPrimaryKeyColumn(string name, Type type)
        {
            var script = _baseScript
                + $" ({name} {PrimaryKeyType(type)} not null, "
                + $"CONSTRAINT PK_{_name} PRIMARY KEY NONCLUSTERED ({name}))";

            BeforeScripts.Enqueue(script);
            return this;
        }

        public Table AddColumn(string name, Type type)
        {
            Scripts.Enqueue(
                $@"IF NOT EXISTS (SELECT * FROM sys.columns
                WHERE object_id = OBJECT_ID(N'[dbo].[{_name}]')
                AND name = '{name}')
                ALTER TABLE {_name}
                ADD {name} {DotnetTypeToSqlType(type)}"
            );
            return this;
        }


        internal override void Validate()
        {
            if (BeforeScripts.Count < 1)
                throw new InvalidOperationException($"Table {_name} has no Primary Key specified.");
        }

        private string PrimaryKeyType(Type type)
        {
            switch (type.Name.ToLower())
            {
                case "int":
                case "int32":
                    return "int identity(1,1)";

                case "guid":
                    return "string";

                default:
                    throw new ArgumentOutOfRangeException("Primary Key type must be int or Guid.");
            }
        }

        private string DotnetTypeToSqlType(Type type)
        {
            switch (type.Name.ToLower())
            {
                case "int":
                case "int32":
                    return "int";
                case "int16":
                    return "smallint";
                case "float":
                case "double":
                    return "float";
                case "decimal":
                    return "decimal";
                case "string":
                    return "nvarchar(max) null";
                case "bool":
                    return "bit";
                case "datetime":
                    return "datetime";
                case "datetimeoffset":
                    return "datetimeoffset";

                default:
                    throw new ArgumentOutOfRangeException($"Cannot translate type {type} to SQL type.");
            }
        }
    }
}