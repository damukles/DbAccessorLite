using System;

namespace DbAccessorLite.Migrations
{
    public interface IDbMigrator
    {
        IDbMigrator AddScript(string script);
        IDbMigrator AddTable(string name, Action<Table> configureTable);
        IDbMigrator RemoveTable(string name);
        void Migrate();
    }
}