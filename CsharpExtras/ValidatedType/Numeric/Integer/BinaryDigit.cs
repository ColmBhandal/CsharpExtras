using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.ValidatedType.Numeric.Integer
{
    public class BinaryDigit : ImmutableValidated<byte>
    {
        public BinaryDigit(byte value) : base(value)
        {
        }

        protected override string ValidityConditionTextDescription => "Binary digit must be 0 or 1";

        protected override bool IsValid(byte t)
        {
            return (t == 0) || (t == 1);
        }

        public static explicit operator BinaryDigit(byte value) => new BinaryDigit(value);
    }
}
