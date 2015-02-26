using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using STSdb4.WaterfallTree;

namespace STSdb4.Remote.Commands
{
    public interface ICommand
    {
        int Code { get; }
        bool IsSynchronous { get; }
    }
}
