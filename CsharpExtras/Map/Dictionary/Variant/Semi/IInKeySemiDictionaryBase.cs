using CsharpExtras.Map.Dictionary.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Variant.Semi
{
    public interface IInKeySemiDictionaryBase<in K> : IDictionaryBase
    {
        bool ContainsKey(K key);
        bool Remove(K key);
    }
}
