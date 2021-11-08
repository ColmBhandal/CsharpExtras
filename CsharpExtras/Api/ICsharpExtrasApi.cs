using CsharpExtras.Compare;
using CsharpExtras.Map.Dictionary;
using CsharpExtras.Enumerable.NonEmpty;
using CsharpExtras.Enumerable.OneBased;
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
    }
}