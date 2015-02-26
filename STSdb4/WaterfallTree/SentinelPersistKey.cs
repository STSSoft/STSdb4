using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using STSdb4.General.Persist;
using STSdb4.Data;
using STSdb4.WaterfallTree;

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
