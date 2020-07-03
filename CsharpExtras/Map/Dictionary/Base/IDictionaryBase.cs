using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Base
{
    public interface IDictionaryBase
    {
        int Count { get; }
        bool IsReadOnly { get; }
        void Clear();
    }
}
