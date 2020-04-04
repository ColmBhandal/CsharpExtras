using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Validation.Impl
{
    public abstract class ValidatorBase<T> : IValidator<T>
    {
        public abstract IValidationErrorCollection Validate(T value);

        public IValidationErrorCollection Validate(IEnumerable<T> values)
        {
            IValidationErrorCollection errorCollection = new ValidationErrorCollectionImpl();
            foreach (T t in values)
            {
                IValidationErrorCollection errors = Validate(t);
                errorCollection.AddAll(errors);
            }

            return errorCollection;
        }
    }
}
