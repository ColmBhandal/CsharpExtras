using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Validation
{
    class ValidationErrorImpl : IValidationError
    {
        public bool IsBlocker { get; }

        public string Message { get; }

        public string DisplayName => IsBlocker ? string.Format("[BLOCKER] {0}", Message) : Message;

        public ValidationErrorImpl(bool isBlocker, string message)
        {
            IsBlocker = isBlocker;
            Message = message;
        }
    }
}
