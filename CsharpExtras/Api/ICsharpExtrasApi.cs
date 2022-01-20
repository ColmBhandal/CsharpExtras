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
        ICurryDictionary<TKeyOuter, TValOuter> NewGenericCurryDictionaryWrapper<TKeyInner, TKeyOuter, TValInner, TValOuter>
            (ICurryDictionary<TKeyInner, TValInner> backingDictionary,
            Func<TKeyOuter, int, TKeyInner> keyInTransform,
            Func<TKeyInner, int, TKeyOuter> keyOutTransform,
            Func<TValOuter, TValInner> valInTransform,
            Func<TValInner, TValOuter> valOutTransform);
        ISparseArrayBuilder<TVal> NewSparseArrayBuilder<TVal>(PositiveInteger dimension, TVal defaultValue);
    }
}