﻿using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Dictionary.Collection
{
    public interface IListValuedDictionary<TKey, TVal> : ICollectionValuedDictionary<TKey, TVal, IList<TVal>>
    {
        /// <summary>
        /// Adds the value at the given index to the list at the given key.
        /// </summary>       
        void InsertAtIndex(TKey key, TVal val, NonnegativeInteger index);

        IListValuedDictionary<TKey, TOther> TransformValues<TOther>(Func<TVal, TOther> transformer);
    }
}
