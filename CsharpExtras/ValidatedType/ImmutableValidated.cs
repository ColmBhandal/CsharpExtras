using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.ValidatedType
{
    /// <summary>
    /// A wrapper class for a generically typed object which validates that object upon construction.
    /// The reference to the wrapped object cannot change, meaning if the object is immutable, then so is this class.
    /// More generally, if the validation condition represents an invariant on the wrapper object, then it will always hold
    /// for the liftetime of the validated object, provided the object itself is immutable.
    /// The validation condition is abstract in this class, meaning it must be implemented for a sub-class.
    /// It is up to the architect of the sub-class to choose an appropriate validation condition.
    /// </summary>
    /// <typeparam name="TVal">This type should be immutable i.e. objects of this type should not change after construction.
    /// There is no compile-time enforcement of this immutability - it is up to the architect of the sub-class to guarantee it.</typeparam>
    public abstract class ImmutableValidated<TVal>
        where TVal : IComparable
    {
        private readonly TVal _val;
        protected ImmutableValidated(TVal value)
        {
            if (IsValid(value))
            {
                _val = value;
            }
            else
            {
                string valueAsString = GetValueAsString(value);
                string message = string.Format("The value {0} is invalid. {1}",
                    valueAsString, ValidityConditionTextDescription);
                throw new ArgumentException(message);
            }
        }

        private string GetValueAsString(TVal value)
        {
            return value?.ToString() ?? "NULL";
        }

        public static implicit operator TVal(ImmutableValidated<TVal> validated) => validated._val;

        protected abstract bool IsValid(TVal t);
        protected abstract string ValidityConditionTextDescription { get; }

        public override string ToString() => GetValueAsString(_val);

        public override int GetHashCode()
        {
            return _val.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            return (obj is ImmutableValidated<TVal> validated &&
                   EqualityComparer<TVal>.Default.Equals(_val, validated._val))
                   ||
                   (obj is TVal val &&
                   EqualityComparer<TVal>.Default.Equals(_val, val));
        }

        public static bool operator ==(ImmutableValidated<TVal> thisObj, ImmutableValidated<TVal> other) =>
            thisObj.Equals(other);
        public static bool operator !=(ImmutableValidated<TVal> thisObj, ImmutableValidated<TVal> other) =>
            !(thisObj == other);

    }
}
