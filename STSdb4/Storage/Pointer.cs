using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace STSdb4.Storage
{
    public class Pointer
    {
        public long Version;
        public Ptr Ptr;

        public bool IsReserved;
        public int RefCount;

        public Pointer(long version, Ptr ptr)
        {
            Version = version;
            Ptr = ptr;
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Version);
            Ptr.Serialize(writer);
        }

        public static Pointer Deserialize(BinaryReader reader)
        {
            long version = reader.ReadInt64();
            Ptr ptr = Ptr.Deserialize(reader);

            return new Pointer(version, ptr);
        }

        public override string ToString()
        {
            return String.Format("Version {0}, Ptr {1}", Version, Ptr);
        }
    }
}
