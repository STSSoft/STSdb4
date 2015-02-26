using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace STSdb4.Data
{
    public interface IToString<T> : ITransformer<T, string>
    {
    }
}
