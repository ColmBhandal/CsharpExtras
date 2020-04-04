using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Validation
{
    public interface IValidationErrorCollection : ICollection<IValidationError>
    {
        bool HasBlockers { get; }

        IEnumerable<string> Messages { get; }

        void AddNewBlocker(string message);

        void AddNewNonBlocker(string message);

        void AddAll(IEnumerable<IValidationError> values);
        string MessagesJoined(string seperator);
    }
}
