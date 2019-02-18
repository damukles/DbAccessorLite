using System.Collections.Generic;
using System.Linq;

namespace DbAccessorLite.Migrations
{
    internal class ScriptArtifact : DbArtifact
    {
        internal ScriptArtifact(string script)
        {
            Scripts.Enqueue(script);
        }

        internal override void Validate()
        {
        }
    }
}