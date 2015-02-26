using STSdb4.Data;
using STSdb4.Database;
using STSdb4.General.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
