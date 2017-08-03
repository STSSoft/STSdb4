using STSdb4.Data;

namespace STSdb4.WaterfallTree
{
    public enum OperationScope : byte
    {
        Point,
        Range,
        Overall
    }

    public interface IOperation
    {
        int Code { get; }
        OperationScope Scope { get; }

        IData FromKey { get; }
        IData ToKey { get; }
    }
}