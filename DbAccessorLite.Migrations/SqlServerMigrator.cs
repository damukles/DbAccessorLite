using System;
using System.Collections.Generic;
using System.Linq;
using DbAccessorLite.Nano;

namespace DbAccessorLite.Migrations
{
    public class SqlServerMigrator : IDbMigrator
    {
        private readonly string _connectionString;

        private Queue<DbArtifact> Artifacts { get; set; } = new Queue<DbArtifact>();

        public SqlServerMigrator(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbMigrator AddScript(string script)
        {
            Artifacts.Enqueue(new ScriptArtifact(script));
            return this;
        }

        public IDbMigrator AddTable(string name, Action<Table> configureTable)
        {
            var table = new Table(name);
            configureTable(table);
            Artifacts.Enqueue(table);
            return this;
        }

        public IDbMigrator RemoveTable(string name)
        {
            Artifacts.Enqueue(new ScriptArtifact($"if exists (select * from sys.tables where name='{name}') drop table {name}"));
            return this;
        }

        public void Migrate()
        {
            foreach (var artifact in Artifacts)
                artifact.Validate();

            var db = new DbAccessorNano(_connectionString);
            foreach (var artifact in Artifacts)
            {
                foreach (var script in artifact.BeforeScripts)
                {
                    db.Execute(script);
                }
                foreach (var script in artifact.Scripts)
                {
                    db.Execute(script);
                }
                foreach (var script in artifact.AfterScripts)
                {
                    db.Execute(script);
                }
            }
        }
    }
}
