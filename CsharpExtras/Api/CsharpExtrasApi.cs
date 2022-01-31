using CsharpExtras.Compare;
using CsharpExtras.Map.Dictionary;
using CsharpExtras.Map.Dictionary.Collection;
using CsharpExtras._Enumerable.NonEmpty;
using CsharpExtras._Enumerable.OneBased;
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
using CsharpExtras._Enumerable.Provider.Int;
using CsharpExtras.Event.Notify;
using CsharpExtras.Event.Wrapper;
using CsharpExtras.Map.Dictionary.Curry.Wrapper;
using CsharpExtras.Map.Sparse.Builder;
using CsharpExtras.Map.Sparse.TwoDimensional.Builder;

namespace CsharpExtras.Api
{
    /// <summary>
    /// Main entry point for using this library. Create a new API in order to create any of the types in this library.
    /// </summary>
    public class CsharpExtrasApi : ICsharpExtrasApi
    {

        /// <summary>
        /// Creates a new curry dictionary with the given arity
        /// </summary>
        /// <param name="arity">The arity of the key-tuples to index this dictionary. Must be positive.</param>
        public ICurryDictionary<TKey, TVal> NewCurryDictionary<TKey, TVal>(int arity)
        {
            return NewCurryDictionary<TKey, TVal>((PositiveInteger) arity);
        }

        /// <summary>
        /// Creates a new curry dictionary with the given arity
        /// </summary>
        /// <param name="arity">The arity of the key-tuples to index this dictionary. Must be positive.</param>
        public ICurryDictionary<TKey, TVal> NewCurryDictionary<TKey, TVal>(PositiveInteger arity)
        {
            return new CurryDictionaryRecursive<TKey, TVal>(arity, this);         
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
            return array.Map((_, i, j) => initialiser(i, j));
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
            return array.Map((x, i) => initialiser(i));
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

        public IVariantDictionary<TKey, TVal> NewVariantDictionary<TKey, TVal>(IDictionary<TKey, TVal> backingDictionary) =>
            new VariantDictionaryImpl<TKey, TVal>(backingDictionary);

        public IUpdateNotifier<TVal, TUpdate> NewUpdateNotifier<TVal, TUpdate>(TVal val,
            Func<TVal, TUpdate, TVal> updater) => new UpdateNotifierImpl<TVal, TUpdate>(val, updater);

        public IEventObjWrapper<TObj, TEvent> NewEventObjWrapper<TObj, TEvent>(TObj obj, Action<TEvent> handler)
            => new EventObjWrapperImpl<TObj, TEvent>(obj, handler);

        public IPropertyChangedWrapper<TObj, TEvent> NewPropertyChangedWrapper
            <TObj, TBefore, TAfter, TEvent>(TObj obj, Func<TObj, TBefore> beforeGetter,
            Func<TObj, TAfter> afterGetter, Func<TBefore, TAfter, TEvent> eventGenerator) =>
            new PropertyChangedWrapperImpl<TObj, TBefore, TAfter, TEvent>(obj, beforeGetter, afterGetter, eventGenerator);

        public ICurryDictionary<TKeyOuter, TValOuter> NewGenericCurryDictionaryWrapper<TKeyInner, TKeyOuter, TValInner, TValOuter>
            (ICurryDictionary<TKeyInner, TValInner> backingDictionary,
            Func<TKeyOuter, int, TKeyInner> keyInTransform,
            Func<TKeyInner, int, TKeyOuter> keyOutTransform,
            Func<TValOuter, TValInner> valInTransform,
            Func<TValInner, TValOuter> valOutTransform) =>
            new GenericCurryDictionaryWrapperImpl<TKeyInner, TKeyOuter, TValInner, TValOuter>
            (backingDictionary, keyInTransform, keyOutTransform, valInTransform, valOutTransform, this);

        public ISparseArrayBuilder<TVal> NewSparseArrayBuilder<TVal>(PositiveInteger dimension, TVal defaultValue) =>
            new SparseArrayBuilderImpl<TVal>(defaultValue, dimension, this);

        public ISparseArray2DBuilder<TVal> NewSparseArray2DBuilder<TVal>(TVal defaultValue) =>
            new SparseArray2DBuilderImpl<TVal>(defaultValue, this);

        public IPreAccessWrapper<TObj> NewPreAccessWrapper<TObj>(TObj obj, Action<TObj> preAccessAction) =>
            new PreAccessWrapperImpl<TObj>(obj, preAccessAction);
    }
}
