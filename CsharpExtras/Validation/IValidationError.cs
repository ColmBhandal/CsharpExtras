using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Validation
{
    public interface IValidationError
    {
        bool IsBlocker { get; }
        string Message { get; }
        string DisplayName { get; }
    }
}
