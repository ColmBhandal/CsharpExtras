using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation
{
    public interface IValidationVisualizer
    {
        //non-mvp: Rename this to something the better describes function's purpose e.g. ShouldContinueAndMaybePromptUser
        /// <summary>
        /// Show errors to the user and prompt the user whether to continue or not. If there are blocker
        /// errors this will always return false.
        /// </summary>
        bool ShowValidationsAndPromptUser(IValidationErrorCollection validations);
    }
}
