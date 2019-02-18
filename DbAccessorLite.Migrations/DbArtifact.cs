using System.Collections.Generic;

namespace DbAccessorLite.Migrations
{
    public abstract class DbArtifact
    {
        internal Queue<string> BeforeScripts { get; set; } = new Queue<string>();
        internal Queue<string> Scripts { get; set; } = new Queue<string>();
        internal Queue<string> AfterScripts { get; set; } = new Queue<string>();

        internal abstract void Validate();
    }
}