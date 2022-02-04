using CsharpExtras.Api;
using CsharpExtras.Compare;
using CsharpExtras.Extensions;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.Map.Sparse.Builder;
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

        public IEnumerable<(IList<int>, TVal)> UsedEntries => 
            _backingDictionary.KeyValuePairs.Select(((IList<ValidIndex> l, TVal v) pair)
                => (pair.l.Map(KeyInTransform), pair.v));

        public bool IsValid(int index, NonnegativeInteger axisIndex) =>
            _validIndexCache[(index, axisIndex)] != null;

        public TVal this[params int[] coordinates]
        {
            get => GetValue(coordinates);
            set => SetValue(coordinates, value);
        }

        public TVal GetValueFromCoordinates(IEnumerable<int> coordinates) =>
            GetValue(coordinates.ToArray());

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
        private readonly ICsharpExtrasApi _api;
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
            _api = api;
            DefaultValue = defaultValue;
            _validationFunction = validationFunction;
            _backingDictionary = api.NewGenericCurryDictionaryWrapper(
                api.NewCurryDictionary<int, TVal>(Dimension), KeyInTransform, KeyOutTransform, v => v, v => v);
            _validIndexCache = api.NewLazyFunctionMap<(int index, int axisIndex), SparseArrayImpl<TVal>.ValidIndex?>
                ((p) => SparseArrayImpl<TVal>.ValidIndex.GetValidIndexOrNull(p.index, (NonnegativeInteger)p.axisIndex, this));
        }

        public bool IsUsed(params int[] coordinates) =>
            _backingDictionary.ContainsKeyTuple(coordinates.Map(KeyOutTransform));

        public bool IsUsed(IEnumerable<int> coordinates) =>
            _backingDictionary.ContainsKeyTuple(coordinates.Select(KeyOutTransform));


        public ISparseArray<TResult> Zip<TOther, TResult>(Func<TVal, TOther, TResult> zipper,
            ISparseArray<TOther> other, TResult defaultVal,
            Func<NonnegativeInteger, int, bool> validationFunction)
        {
            int otherDimension = other.Dimension;
            if (Dimension != otherDimension)
            {
                throw new ArgumentException($"Cannot zip arrays. Dimension mismatch. This array has dimension " +
                    $"{Dimension} while the other has dimension {otherDimension}");
            }
            ISparseArray<TResult> result = _api.NewSparseArrayBuilder(Dimension, defaultVal)
                .Build();
            foreach((IList<int> coordinatesList, TVal val) in UsedEntries)
            {
                int[] coordinates = coordinatesList.ToArray();
                TOther otherVal = other[coordinates];
                TResult zipped = zipper(val, otherVal);
                result[coordinates] = zipped;
            }
            foreach ((IList<int> coordinatesList, TOther otherVal) in other.UsedEntries)
            {
                if (!result.IsUsed(coordinatesList))
                {
                    int[] coordinates = coordinatesList.ToArray();
                    TVal val = this[coordinates];
                    TResult zipped = zipper(val, otherVal);
                    result[coordinates] = zipped;
                }
            }
            return result;
        }

        public IComparisonResult CompareUsedValues(ISparseArray<TVal> other, Func<TVal, TVal, bool> valueComparer)
        {
            if (other.Dimension != Dimension)
            {
                return new SparseArrayComparisonImpl<TVal>(Dimension, other.Dimension, UsedValuesCount, other.UsedValuesCount, null, null);
            }
            if (other.UsedValuesCount != UsedValuesCount)
            {
                return new SparseArrayComparisonImpl<TVal>(Dimension, other.Dimension, UsedValuesCount, other.UsedValuesCount, null, null);
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
                        (transformedKeyTupleEnum.ToList(), thisValue), otherValue?.ToString());
                }
            }
            return new SparseArrayComparisonImpl<TVal>(Dimension, other.Dimension, UsedValuesCount, other.UsedValuesCount, null, null);
        }

        public void Shift(NonnegativeInteger axisIndex, int firstShiftIndex, int shiftVector) =>
            _backingDictionary.UpdateKeys
                (k => KeyOutTransform(ShiftIfInRange(k, firstShiftIndex, shiftVector), axisIndex), axisIndex);

        /// <summary>
        /// Shifts the given index if it is in the interval defined by the first shift index and shift vector
        /// </summary>
        /// <param name="index">The index to shift</param>
        /// <param name="firstShiftIndex">The first index to shift</param>
        /// <param name="shiftVector">The amount by which to shift.</param>
        /// <returns>If the given index is within the semi-infinite interval starting at firstShiftIndex, and
        /// extending out to plus or minus infinity depending on the shift vector, then translates it by the shift amount.
        /// Otherwise, returns the same index that was input to the function.
        /// If the shift vector is zero, then this returns the given index.</returns>
        private int ShiftIfInRange(int index, int firstShiftIndex, int shiftVector)
        {
            if((shiftVector < 0 && index <= firstShiftIndex) 
                || (shiftVector > 0 && index >= firstShiftIndex))
            {
                return index + shiftVector;
            }
            return index;
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
