using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.ValidatedType
{
    /// <summary>
    /// A wrapper class for a generically typed object which validates that object upon construction.
    /// The reference to the wrapped object cannot change, meaning if the object is immutable, then so is this class.
    /// More generally, if the validation condition represents an invariant on the wrapper object, then it will always hold
    /// for the liftetime of the validated object.
    /// The validation condition is abstract in this class, meaning it must be implemented for a sub-class.
    /// It is up to the architect of the sub-class to choose an appropriate validation condition.
    /// </summary>    
    public abstract class Validated<TVal>
    {
        private readonly TVal _value;
        protected Validated(TVal value)
        {
            if (IsValid(value))
            {
                _value = value;
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

        public static implicit operator TVal(Validated<TVal> validated) => validated._value;

        protected abstract bool IsValid(TVal t);
        protected abstract string ValidityConditionTextDescription { get; }

        public override string ToString() => GetValueAsString(_value);
    }
}
