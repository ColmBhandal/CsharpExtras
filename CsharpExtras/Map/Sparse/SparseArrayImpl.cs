using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Extensions;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.Map.Sparse.Compare;
using CsharpExtras.ValidatedType;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Map.Sparse
{
    internal class SparseArrayImpl<TVal> : ISparseArray<TVal>
    {
        public PositiveInteger Dimension { get; }
        public TVal DefaultValue { get; }

        public NonnegativeInteger UsedValuesCount => _backingDictionary.Count;

        public bool IsValid(int index, NonnegativeInteger axisIndex) =>
            _validIndexCache[(index, axisIndex)] != null;

        public TVal this[params int[] coordinates]
        {
            get => GetValue(coordinates);
            set => SetValue(coordinates, value);
        }

        private void SetValue(int[] coordinates, TVal value)
        {
            ValidIndex[] validatedCoordinates = GetValidateCoordinated(coordinates);
            if (EqualityComparer<TVal>.Default.Equals(value, DefaultValue))
            {
                return;
            }
            if (!_backingDictionary.Update(value, validatedCoordinates))
            {
                _backingDictionary.Add(value, validatedCoordinates);
            }
        }

        private TVal GetValue(int[] coordinates)
        {
            ValidIndex[] validatedCoordinates = GetValidateCoordinated(coordinates);
            if (_backingDictionary.ContainsKeyTuple(validatedCoordinates))
            {
                return _backingDictionary[validatedCoordinates];
            }
            return DefaultValue;
        }

        private SparseArrayImpl<TVal>.ValidIndex[] GetValidateCoordinated(int[] coordinates) 
            => coordinates.Map((index, axisIndex) => _validIndexCache[(index, axisIndex)] ??
                throw new IndexOutOfRangeException($"Invalid index {index} for axis {axisIndex}"));

        private readonly ICurryDictionary<ValidIndex, TVal> _backingDictionary;

        private readonly Func<NonnegativeInteger, int, bool> _validationFunction;

        private ILazyFunctionMap<(int index, int axisIndex), SparseArrayImpl<TVal>.ValidIndex?> _validIndexCache;

        /// <summary>
        /// Constructs a new sparse array
        /// </summary>
        /// <param name="dimension">The dimension i.e. number of coordinates per mapping in the sparse array</param>        
        /// <param name="validationFunction">Given an axis index and an index along that axis, returns whether the given index is valid.
        /// Note: this function only needs to handle axis indices from 0 up to the dimension of this array,
        /// even though that parameter is typed for all positive integers</param>
        /// <param name="defaultValue"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SparseArrayImpl(PositiveInteger dimension, ICsharpExtrasApi api,
            Func<NonnegativeInteger, int, bool> validationFunction, TVal defaultValue)
        {
            Dimension = dimension ?? throw new ArgumentNullException(nameof(dimension));
            DefaultValue = defaultValue;
            _validationFunction = validationFunction;
            _backingDictionary = api.NewGenericCurryDictionaryWrapper(
                api.NewCurryDictionary<int, TVal>(Dimension), KeyInTransform, KeyOutTransform, v => v, v => v);
            _validIndexCache = api.NewLazyFunctionMap<(int index, int axisIndex), SparseArrayImpl<TVal>.ValidIndex?>
                ((p) => SparseArrayImpl<TVal>.ValidIndex.GetValidIndexOrNull(p.index, (NonnegativeInteger)p.axisIndex, this));
        }

        public IComparisonResult CompareUsedValues(ISparseArray<TVal> other, Func<TVal, TVal, bool> valueComparer)
        {
            if (other.Dimension != Dimension)
            {
                return new SparseArrayComparisonImpl<TVal>(Dimension, other.Dimension, UsedValuesCount, other.UsedValuesCount, null);
            }
            if (other.UsedValuesCount != UsedValuesCount)
            {
                return new SparseArrayComparisonImpl<TVal>(Dimension, other.Dimension, UsedValuesCount, other.UsedValuesCount, null);
            }            
            foreach(IList<SparseArrayImpl<TVal>.ValidIndex> tuple in _backingDictionary.KeyTuples)
            {
                IEnumerable<int> transformedKeyTupleEnum = tuple.Select(i => (int)i);
                int[] transformedKeyTuple = transformedKeyTupleEnum.ToArray();
                TVal otherValue = other[transformedKeyTuple];
                TVal thisValue = this[transformedKeyTuple];
                if (EqualityComparer<TVal>.Default.Equals(otherValue, other.DefaultValue)
                    || !valueComparer(otherValue, thisValue))
                {
                    return new SparseArrayComparisonImpl<TVal>(Dimension, other.Dimension, UsedValuesCount, other.UsedValuesCount,
                        (transformedKeyTupleEnum.ToList(), thisValue));
                }
            }
            return new SparseArrayComparisonImpl<TVal>(Dimension, other.Dimension, UsedValuesCount, other.UsedValuesCount, null);
        }

        private int KeyInTransform(SparseArrayImpl<TVal>.ValidIndex index, int axisIndex) => index;

        private SparseArrayImpl<TVal>.ValidIndex KeyOutTransform(int index, int axisIndex) => _validIndexCache[(index, axisIndex)]
            ?? throw new IndexOutOfRangeException($"Invalid index {index} for axis {axisIndex}");

        private class ValidIndex
        {
            private readonly int _index;

            /// <returns>If the index is valid according to the axis and array, returns a valid index instance. Else returns null.</returns>
            public static ValidIndex? GetValidIndexOrNull(int index, NonnegativeInteger axisIndex, SparseArrayImpl<TVal> array)
            {
                if (!IsValid(index, axisIndex, array))
                {
                    return null;
                }
                return new ValidIndex(index);
            }

            private ValidIndex(int index)
            {
                _index = index;
            }

            public static implicit operator int(ValidIndex validIndex) => validIndex._index;

            private static bool IsValid(int index, NonnegativeInteger axisIndex, SparseArrayImpl<TVal> array)
            {                
                return array._validationFunction(axisIndex, index);
            }
        }
    }
}
