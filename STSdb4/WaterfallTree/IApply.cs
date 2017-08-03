using STSdb4.Data;
using STSdb4.General.Collections;

namespace STSdb4.WaterfallTree
{
    public interface IApply
    {
        /// <summary>
        /// Compact the operations and returns true, if the collection was modified.
        /// </summary>
        bool Internal(IOperationCollection operations);

        bool Leaf(IOperationCollection operations, IOrderedSet<IData, IData> data);

        Locator Locator { get; }
    }
}
