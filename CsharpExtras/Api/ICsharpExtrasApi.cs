using CsharpExtras.Compare;
using CsharpExtras.Map.Dictionary;
using CsharpExtras._Enumerable.NonEmpty;
using CsharpExtras._Enumerable.OneBased;
using CsharpExtras.IO;
using CsharpExtras.Map;
using CsharpExtras.Log;
using System;
using System.Collections.Generic;
using CsharpExtras.Tree.Integer;
using CsharpExtras.Tree.Base;
using CsharpExtras.RandomDataGen;
using CsharpExtras.Validation;
using CsharpExtras.Map.Dictionary.Collection;
using CsharpExtras.Map.Dictionary.Variant;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.ValidatedType.Numeric.Integer;
using CsharpExtras._Enumerable.Provider.Int;
using CsharpExtras.Event.Notify;
using CsharpExtras.Event.Wrapper;
using CsharpExtras.Map.Sparse.Builder;

namespace CsharpExtras.Api
{
    public interface ICsharpExtrasApi
    {
        IBijectionDictionary<TKey, TVal> NewBijectionDictionary<TKey, TVal>();
        IComparer<T> NewDescendingComparer<T>();
        IDirectoryDecorator NewDirectoryDecorator();
        IValidator<T> NewEmptyValidator<T>();
        IFileDecorator NewFileDecorator();
        IIntegerLeaf NewIntegerLeaf(int payload);
        IIntegerTree NewIntegerTree(int payload);
        ILazyFunctionMap<TKey, TVal> NewLazyFunctionMap<TKey, TVal>(Func<TKey, TVal> backingFunction);
        ILeafBase<TVal> NewLeaf<TVal>(TVal payload);
        INonEmptyCollection<TVal> NewNonEmptyCollection<TVal>(TVal val);
        INonEmptyEnumerable<TVal> NewNonEmptyEnumerable<TVal>(TVal val);
        IOneBasedArray<TVal> NewOneBasedArray<TVal>(TVal[] zeroBasedBackingArray);
        IOneBasedArray<TVal> NewOneBasedArray<TVal>(int size);
        IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(TVal[,] zeroBasedBackingArray);
        IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(int rows, int columns);
        IPathDecorator NewPathDecorator();
        IRandomStringGenerator NewRandomStringGenerator();
        IRegexPatternDictionary<TVal> NewRegexPatternDictionary<TVal>();
        IValidationError NewValidationError(bool isBlocker, string message);
        IValidationErrorCollection NewValidationErrorCollection();
        void SetLogger(ILogger logger);
        ISetValuedDictionary<TKey, TVal> NewSetValuedDictionary<TKey, TVal>();
        IListValuedDictionary<TKey, TVal> NewListValuedDictionary<TKey, TVal>();
        IVariantDictionary<TKey, TVal> NewVariantDictionary<TKey, TVal>();
        IVariantDictionary<TKey, TVal> NewVariantDictionary<TKey, TVal>(IDictionary<TKey, TVal> backingDictionary);
        ICurryDictionary<TKey, TVal> NewCurryDictionary<TKey, TVal>(int arity);
        ICurryDictionary<TKey, TVal> NewCurryDictionary<TKey, TVal>(PositiveInteger arity);
        ISequentialIntProvider NewSequentialIntProvider(int start, int step);
        IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(int rows, int columns, Func<int, int, TVal> initialiser);
        IOneBasedArray<TVal> NewOneBasedArray<TVal>(int size, Func<int, TVal> initialiser);
        IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(int rows, int columns, TVal initialValue);
        IOneBasedArray<TVal> NewOneBasedArray<TVal>(int size, TVal initialValue);
        IUpdateNotifier<TVal, TUpdate> NewUpdateNotifier<TVal, TUpdate>(TVal val, Func<TVal, TUpdate, TVal> updater);
        IEventObjWrapper<TObj, TEvent> NewEventObjWrapper<TObj, TEvent>(TObj obj, Action<TEvent> handler);
        IPropertyChangedWrapper<TObj, TEvent> NewPropertyChangedWrapper<TObj, TBefore, TAfter, TEvent>
            (TObj obj, Func<TObj, TBefore> beforeGetter, Func<TObj, TAfter> afterGetter, Func<TBefore, TAfter, TEvent> eventGenerator);

        /// <summary>
        /// Creates a new curry dictionary wrapper. A curry dictionary wrapper is a curry dictionary which is backed by another curry dictionary
        /// and exposes all of its methods through conversion functions which convert keys and value to/from the original dictionary.
        /// The backing dictionary can be referred to as the inner dictionary and the wrapper as the outer dictionary.
        /// Note: a sensible usage of this wrapper is to make the pairs of key/value transforms bijections. Non-bijective transform pairs may give strange results.
        /// </summary>
        /// <typeparam name="TKeyInner">The key type of the inner dictionary</typeparam>
        /// <typeparam name="TKeyOuter">The key type of the outer dictionary</typeparam>
        /// <typeparam name="TValInner">The value type of the inner dictionary</typeparam>
        /// <typeparam name="TValOuter">The value type of the outer dictionary</typeparam>
        /// <param name="backingDictionary">The inner dictionary.</param>
        /// <param name="keyInTransform">A transformation function that converts an outer key and an index to an inner key.
        /// The index is the index of the key in a key tuple. Keys at different indices may be transformed differently.</param>
        /// <param name="keyOutTransform">A transformation function that converts an inner key and an index to an outer key.
        /// The index is the index of the key in a key tuple. Keys at different indices may be transformed differently.</param>
        /// <param name="valInTransform">A transformation function that converts outer values to inner values</param>
        /// <param name="valOutTransform">A transformation function that converts inner values to outer values</param>
        /// <returns>A new curry dictionary, which wraps around the backing dictionary and exposes the new key/value type</returns>
        ICurryDictionary<TKeyOuter, TValOuter> NewGenericCurryDictionaryWrapper<TKeyInner, TKeyOuter, TValInner, TValOuter>
            (ICurryDictionary<TKeyInner, TValInner> backingDictionary,
            Func<TKeyOuter, int, TKeyInner> keyInTransform,
            Func<TKeyInner, int, TKeyOuter> keyOutTransform,
            Func<TValOuter, TValInner> valInTransform,
            Func<TValInner, TValOuter> valOutTransform);
        ISparseArrayBuilder<TVal> NewSparseArrayBuilder<TVal>(PositiveInteger dimension, TVal defaultValue);
    }
}