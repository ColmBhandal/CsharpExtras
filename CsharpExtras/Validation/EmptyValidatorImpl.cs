using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Validation
{
    class EmptyValidatorImpl<T> : ValidatorBase<T>
    {
        public override IValidationErrorCollection Validate(T value)
        {
            return new ValidationErrorCollectionImpl();
        }
    }
}
