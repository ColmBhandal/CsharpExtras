using CsharpExtras.Compare;
using CsharpExtras.Map.Dictionary;
using CsharpExtras.Map.Dictionary.Collection;
using CsharpExtras.Enumerable.NonEmpty;
using CsharpExtras.Enumerable.OneBased;
using CsharpExtras.IO;
using CsharpExtras.Log;
using CsharpExtras.Map;
using CsharpExtras.RandomDataGen;
using CsharpExtras.Tree.Base;
using CsharpExtras.Tree.Integer;
using CsharpExtras.Validation;
using System;
using System.Collections.Generic;
using CsharpExtras.Map.Dictionary.Variant;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.ValidatedType.Numeric.Integer;
using CsharpExtras.Enumerable.Provider.Int;

namespace CsharpExtras.Api
{
    public class CsharpExtrasApi : ICsharpExtrasApi
    {
        public ICurryDictionary<TKey, TVal> NewCurryDictionary<TKey, TVal>(int arity)
        {
            return NewCurryDictionary<TKey, TVal>((PositiveInteger) arity);
        }
        public ICurryDictionary<TKey, TVal> NewCurryDictionary<TKey, TVal>(PositiveInteger arity)
        {
            return new CurryDictionaryRecursive<TKey, TVal>(arity);         
        }
        public void SetLogger(ILogger logger)
        {
            StaticLogManager.Logger = logger;
        }

        public IListValuedDictionary<TKey, TVal> NewListValuedDictionary<TKey, TVal>()
        {
            return new ListValuedDictionaryImpl<TKey, TVal>();
        }

        public IComparer<T> NewDescendingComparer<T>() => new DescendingComparer<T>();

        public IBijectionDictionary<TKey, TVal> NewBijectionDictionary<TKey, TVal>()
            => new BijectionDictionaryImpl<TKey, TVal>();
        public IRegexPatternDictionary<TVal> NewRegexPatternDictionary<TVal>()
            => new RegexPatternDictionaryImpl<TVal>();
        public ISetValuedDictionary<TKey, TVal> NewSetValuedDictionary<TKey, TVal>()
            => new SetValuedDictionaryImpl<TKey, TVal>();

        public INonEmptyCollection<TVal> NewNonEmptyCollection<TVal>(TVal val)
            => new NonEmptyCollectionImpl<TVal>(val);

        public INonEmptyEnumerable<TVal> NewNonEmptyEnumerable<TVal>(TVal val)
            => new NonEmptyEnumerableImpl<TVal>(val);

        public IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(TVal[,] zeroBasedBackingArray)
        {
            return new OneBasedArray2DImpl<TVal>(zeroBasedBackingArray);
        }
        public IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(int rows, int columns)
        {
            return new OneBasedArray2DImpl<TVal>(rows, columns);
        }
        public IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(int rows, int columns, Func<int, int, TVal> initialiser)
        {
            IOneBasedArray2D<TVal> array = new OneBasedArray2DImpl<TVal>(rows, columns);
            return array.Map((i, j, _) => initialiser(i, j));
        }
        public IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(int rows, int columns, TVal initialValue) =>
            NewOneBasedArray2D(rows, columns, (i, j) => initialValue);

        public IOneBasedArray<TVal> NewOneBasedArray<TVal>(TVal[] zeroBasedBackingArray)
        {
            return new OneBasedArrayImpl<TVal>(zeroBasedBackingArray);
        }

        public IOneBasedArray<TVal> NewOneBasedArray<TVal>(int size)
        {
            return new OneBasedArrayImpl<TVal>(size);
        }
        public IOneBasedArray<TVal> NewOneBasedArray<TVal>(int size, Func<int, TVal> initialiser)
        {
            IOneBasedArray<TVal> array = new OneBasedArrayImpl<TVal>(size);
            return array.Map((i, x) => initialiser(i));
        }

        public IOneBasedArray<TVal> NewOneBasedArray<TVal>(int size, TVal initialValue) =>
            NewOneBasedArray(size, i => initialValue);

        public ISequentialIntProvider NewSequentialIntProvider(int start, int step)
        {
            return new SequentialIntProviderImpl(start, step);
        }

        public IFileDecorator NewFileDecorator() => new FileDecoratorImpl();
        public IDirectoryDecorator NewDirectoryDecorator() => new DirectoryDecoratorImpl();
        public IPathDecorator NewPathDecorator() => new PathDecoratorImpl();
        public ILazyFunctionMap<TKey, TVal> NewLazyFunctionMap<TKey, TVal>(Func<TKey, TVal> backingFunction)
            => new LazyFunctionMapImpl<TKey, TVal>(backingFunction);
        public IRandomStringGenerator NewRandomStringGenerator() => new RandomStringGeneratorImpl();
        public ILeafBase<TVal> NewLeaf<TVal>(TVal payload) => new LeafBase<TVal>(payload);
        public IIntegerTree NewIntegerTree(int payload) => new IntegerTreeImpl(payload);
        public IIntegerLeaf NewIntegerLeaf(int payload) => new IntegerLeafImpl(payload);
        public IValidationErrorCollection NewValidationErrorCollection() =>
            new ValidationErrorCollectionImpl();
        public IValidationError NewValidationError(bool isBlocker, string message)
            => new ValidationErrorImpl(isBlocker, message);

        public IValidator<T> NewEmptyValidator<T>() => new EmptyValidatorImpl<T>();

        public IVariantDictionary<TKey, TVal> NewVariantDictionary<TKey, TVal>()
        {
            IDictionary<TKey, TVal> backingDictionary = new Dictionary<TKey, TVal>();
            return NewVariantDictionary(backingDictionary);
        }

        public IVariantDictionary<TKey, TVal> NewVariantDictionary<TKey, TVal>(IDictionary<TKey, TVal> backingDictionary)
        {
            return new VariantDictionaryImpl<TKey, TVal>(backingDictionary);
        }
    }
}
