using System.IO;
using STSdb4.General.Persist;
using STSdb4.Data;

namespace STSdb4.WaterfallTree
{
    public class SentinelPersistKey : IPersist<IData>
    {
        public static readonly SentinelPersistKey Instance = new SentinelPersistKey();

        public void Write(BinaryWriter writer, IData item)
        {
        }

        public IData Read(BinaryReader reader)
        {
            return null;
        }
    }
}
