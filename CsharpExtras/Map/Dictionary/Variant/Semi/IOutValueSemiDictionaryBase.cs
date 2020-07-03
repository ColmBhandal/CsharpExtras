using CsharpExtras.Map.Dictionary.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Variant.Semi
{
    public interface IOutValueSemiDictionaryBase<out V> : IDictionaryBase
    {
        IEnumerable<V> Values { get; }
    }
}
