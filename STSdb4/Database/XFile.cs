using STSdb4.Data;

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
