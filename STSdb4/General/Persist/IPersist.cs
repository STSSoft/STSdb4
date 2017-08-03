using System.IO;

namespace STSdb4.General.Persist
{
    public interface IPersist
    {
    }

    public interface IPersist<T> : IPersist
    {
        void Write(BinaryWriter writer, T item);
        T Read(BinaryReader reader);
    }
}
