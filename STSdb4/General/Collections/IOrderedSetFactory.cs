using STSdb4.Data;

namespace STSdb4.General.Collections
{
    public interface IOrderedSetFactory
    {
        IOrderedSet<IData, IData> Create();
    }
}
