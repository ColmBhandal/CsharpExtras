using CsharpExtras.Api;
using CsharpExtras.Extensions;
using CsharpExtras.ValidatedType.Numeric.Integer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsharpExtras.Map.Sparse.Builder
{
    internal class SparseArrayBuilderImpl<TVal> : ISparseArrayBuilder<TVal>
    {
        public TVal DefaultValue { get; }
        public PositiveInteger Dimension { get; }
        private readonly ICsharpExtrasApi _api;

        private readonly Func<int, bool>[] _validationFunctions;

        private readonly ISet<(TVal value, int[] coordinates)> _bufferedWrites
            = new HashSet<(TVal value, int[] coordinates)>();

        public SparseArrayBuilderImpl(TVal defaultValue, PositiveInteger dimension, ICsharpExtrasApi api)
        {
            DefaultValue = defaultValue;
            Dimension = dimension ?? throw new ArgumentNullException(nameof(dimension));
            _api = api ?? throw new ArgumentNullException(nameof(api));
            /*Non-MVP: Replace Enumerable.Repeat with a for-loop backed array population method, which writes a constant value to each index
             * However, this is not really important as the array is typically small - it's the number of dimensions*/
            _validationFunctions = Enumerable.Repeat<Func<int, bool>>(x => true, dimension).ToArray();
        }

        public ISparseArray<TVal> Build()
        {
            Func<int, bool>[] validationFunctionsCopy = _validationFunctions.DeepCopy();
            Func<NonnegativeInteger, int, bool> validationFunction = (i, j) => Validate(validationFunctionsCopy, i, j);
            ISparseArray<TVal> sparseArray = new SparseArrayImpl<TVal>(Dimension, _api, validationFunction, DefaultValue);
            WriteValues(sparseArray);
            return sparseArray;
        }

        private void WriteValues(ISparseArray<TVal> sparseArray)
        {
            foreach((TVal value, int[] coordinates) in _bufferedWrites)
            {
                sparseArray[coordinates] = value;
            }
        }

        private bool Validate(Func<int, bool>[] validationFunctionsCopy, NonnegativeInteger axisIndex, int index)
        {
            if (axisIndex >= Dimension)
            {
                throw new ArgumentException($"Axis index {(int)axisIndex} cannot be larger or equal to dimension {Dimension}");
            }
            Func<int, bool> axisValidator = validationFunctionsCopy[axisIndex];
            return axisValidator(index);
        }

        public ISparseArrayBuilder<TVal> WithValidationFunction(Func<int, bool> indexValidationFunction, NonnegativeInteger axisIndex)
        {
            if(axisIndex >= Dimension)
            {
                throw new ArgumentException($"Axis index {(int)axisIndex} cannot be larger or equal to dimension {Dimension}");
            }
            _validationFunctions[axisIndex] = indexValidationFunction;
            return this;
        }
        
        public ISparseArrayBuilder<TVal> WithValue(TVal value, params int[] coordinates)
        {
            _bufferedWrites.Add((value, coordinates));
            return this;
        }
    }
}
