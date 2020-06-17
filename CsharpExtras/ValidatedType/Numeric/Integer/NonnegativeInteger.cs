using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.ValidatedType.Numeric.Integer
{
    public class NonnegativeInteger : Validated<int>
    {
        public NonnegativeInteger(int value) : base(value)
        {
        }

        protected override string ValidityConditionTextDescription => "Integer must be greater than or equal to zero.";

        protected override bool IsValid(int t) => t >= 0;

        public static explicit operator NonnegativeInteger(int value) => new NonnegativeInteger(value);
    }
}
