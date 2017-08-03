﻿using STSdb4.WaterfallTree;
using STSdb4.Data;

namespace STSdb4.Database.Operations
{
    public abstract class OverallOperation : IOperation
    {
        public OverallOperation(int action)
        {
            Code = action;
        }

        public int Code { get; private set; }

        public OperationScope Scope
        {
            get { return OperationScope.Overall; }
        }

        public IData FromKey
        {
            get { return null; }
        }

        public IData ToKey
        {
            get { return null; }
        }
    }

    public class ClearOperation : OverallOperation
    {
        public ClearOperation()
            : base(OperationCode.CLEAR)
        {
        }
    }
}
