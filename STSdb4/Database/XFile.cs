using STSdb4.Data;
using System;

namespace STSdb4.Database
{
    public class XFile : XStream
    {
        internal XFile(ITable<IData, IData> table)
            : base(table)
        {
        }
    }
}
