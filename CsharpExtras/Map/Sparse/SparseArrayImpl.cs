using CsharpExtras.Api;
using CsharpExtras.Map.Dictionary.Curry;
using CsharpExtras.ValidatedType;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse
{
    internal class SparseArrayImpl<TVal>
    {
        public int IndexBase { get; }

        private readonly ICurryDictionary<ValidIndex, TVal> _backingDictionary;
        private readonly ICsharpExtrasApi _api;



        private class ValidIndex : ImmutableValidated<int>
        {
            //A private constructor ensures that this type controls validation, nobody else
            private ValidIndex(int value) : base(value)
            {
            }

            protected override string ValidityConditionTextDescription =>
                "Ensures an index at a given dimension is valid";

            protected override bool IsValid(int t)
            {
                //TODO: Implement. For efficiency, use a pool - implement it as a dictionary from (dimension, index) => ValidIndex
                return false;
            }
        }
    }
}
