using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.ValidatedType
{
    public abstract class Validated<TVal>
    {
        TVal Value;
        protected Validated(TVal value)
        {
            if (IsValid(value))
            {
                Value = value;
            }
            else
            {
                string valueAsString = value?.ToString() ?? "NULL";
                string message = string.Format("The value {0} is invalid. {1}",
                    valueAsString, ValidityConditionTextDescription);
                throw new ArgumentException(message);
            }
        }

        public static implicit operator TVal(Validated<TVal> validated) => validated.Value;

        protected abstract bool IsValid(TVal t);
        protected abstract string ValidityConditionTextDescription { get; }

    }
}
