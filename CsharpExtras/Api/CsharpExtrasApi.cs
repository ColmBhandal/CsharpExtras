using CsharpExtras.Compare;
using CsharpExtras.Dictionary;
using CsharpExtras.Dictionary.Collection;
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

namespace CsharpExtras.Api
{
    public class CsharpExtrasApi : ICsharpExtrasApi
    {

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
        public ISetValuedDictionary<TKey, TVal> NewSetDictionary<TKey, TVal>()
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
            TVal[,] zeroBased = new TVal[rows, columns];
            return NewOneBasedArray2D(zeroBased);
        }

        public IOneBasedArray<TVal> NewOneBasedArray<TVal>(TVal[] zeroBasedBackingArray)
        {
            return new OneBasedArrayImpl<TVal>(zeroBasedBackingArray);
        }
        public IOneBasedArray<TVal> NewOneBasedArray<TVal>(int size)
        {
            TVal[] zeroBased = new TVal[size];
            return NewOneBasedArray(zeroBased);
        }

        public IFileDecorator NewFileDecorator() => new FileDecoratorImpl(NewFileFacade());

        public IFileFacade NewFileFacade() => new FileFacadeImpl();
        public IDirectoryFacade NewDirectoryFacade() => new DirectoryFacadeImpl();
        public IPathFacade NewPathFacade() => new PathFacadeImpl();
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
    }
}
