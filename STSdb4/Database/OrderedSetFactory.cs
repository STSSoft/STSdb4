using STSdb4.Data;
using STSdb4.General.Collections;
using STSdb4.WaterfallTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STSdb4.Database
{
    public class OrderedSetFactory : IOrderedSetFactory
    {
        public Locator Locator { get; private set; }
        
        public OrderedSetFactory(Locator locator)
        {
            Locator = locator;
        }

        public IOrderedSet<IData, IData> Create()
        {
            var data = new OrderedSet<IData, IData>(Locator.KeyComparer, Locator.KeyEqualityComparer);
            
            return data;
        }
    }
}
