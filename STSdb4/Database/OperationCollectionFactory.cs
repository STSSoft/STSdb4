using STSdb4.WaterfallTree;

namespace STSdb4.Database
{
    public class OperationCollectionFactory : IOperationCollectionFactory
    {
        public readonly Locator Locator;
        
        public OperationCollectionFactory(Locator locator)
        {
            Locator = locator;
        }

        public IOperationCollection Create(int capacity)
        {
            return new OperationCollection(Locator, capacity);
        }

        public IOperationCollection Create(IOperation[] operations, int commonAction, bool areAllMonotoneAndPoint)
        {
            return new OperationCollection(Locator, operations, commonAction, areAllMonotoneAndPoint);
        }
    }
}
