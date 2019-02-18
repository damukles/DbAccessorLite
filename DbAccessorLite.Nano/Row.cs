using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DbAccessorLite.Nano
{
    public class Row : IEnumerable<Column>
    {
        private IEnumerable<Column> data;

        public Row(IEnumerable<Column> data)
        {
            this.data = data;
        }

        public string this[string colName]
        {
            get
            {
                return this.SingleOrDefault(c => c.Name == colName)?.Value;
            }
        }

        public IEnumerator<Column> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)this.GetEnumerator();
        }
    }
}