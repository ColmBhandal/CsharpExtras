using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.ValidatedType.Numeric.Integer
{
    public class PositiveInteger : Validated<int>
    {
        public PositiveInteger(int value) : base(value)
        {
        }

        protected override string ValidityConditionTextDescription => "Integer must be greater than zero.";

        protected override bool IsValid(int t) => t > 0;

        public static explicit operator PositiveInteger(int value) => new PositiveInteger(value);
    }
}
