using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Tree.Integer
{
    interface IChildSetIntegerTree : IChildSetTree<int, IChildSetIntegerTree>
    {
    }
}
