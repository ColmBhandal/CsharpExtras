using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Validation
{
    public interface IValidator<T>
    {
        IValidationErrorCollection Validate(T value);

        IValidationErrorCollection Validate(IEnumerable<T> values);
    }
}
