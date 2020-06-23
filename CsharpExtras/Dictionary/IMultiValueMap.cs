using System;
using System.Collections.Generic;

namespace CsharpExtras.Dictionary
{
    public interface IMultiValueMap<TKey, TVal> : IDictionary<TKey, ISet<TVal>>
    {
        void Add(TKey key, TVal value);

        bool AnyValues();

        /// <summary>
        /// Generates a new multivalue map whose sets are the image of applying the transformer to this map's sets.
        /// </summary>
        /// <typeparam name="TOther">The return type of the transformer function.</typeparam>
        /// <param name="transformer">A function which transforms each value to some other value.</param>
        /// <returns>A new map, with the same keyset as this, whose values are sets resulting from applying the transformer
        /// to all elements of the corresponding set in this map and then aggregating the mapped elements to a set.
        /// Note: the sets in the resuling map may be smaller than those in the original map, if the transformer function maps many-to-one.</returns>
        IMultiValueMap<TKey, TOther> TransformValues<TOther>(Func<TVal, TOther> transformer);
    }
}
