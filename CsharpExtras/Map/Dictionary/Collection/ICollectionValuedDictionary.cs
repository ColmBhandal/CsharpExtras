using System;
using System.Collections.Generic;

namespace CsharpExtras.Map.Dictionary.Collection
{
    public interface ICollectionValuedDictionary<TKey, TVal, TColl> : IDictionary<TKey, TColl>
    {
        void Add(TKey key, TVal value);

        bool AnyValues();
        bool DictEquals(ICollectionValuedDictionary<TKey, TVal, TColl> otherDict);

        /// <summary>
        /// Generates a new dictionary of the given type whose collection values
        /// are the image of applying the transformer to this map's collection values.
        /// </summary>
        /// <typeparam name="TOther">The return type of the transformer function.</typeparam>
        /// <param name="transformer">A function which transforms each value to some other value.</param>
        /// <returns>A new map, with the same keyset as this, whose values are collections resulting from applying the transformer
        /// to all elements of the corresponding colleciton in this map and then aggregating the mapped elements to a collection.
        /// Note: the collections in the resuling map may be smaller than those in the original map,
        /// e.g. if the collections are sets and the transformer function maps many-to-one.</returns>
        TDict TransformValues<TOther, TOtherColl, TDict>(Func<TVal, TOther> transformer,
            Func<TDict> emptyDictionaryGenerator)
            where TDict : ICollectionValuedDictionary<TKey, TOther, TOtherColl>
            where TOtherColl : ICollection<TOther>;
    }
}
