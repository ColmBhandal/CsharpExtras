using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.ValidatedType
{
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
