using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Extensions
{
    public static class BoolExtension
    {
        private const string YesString = "Yes";
        private const string NoString = "No";

        //Non-mvp: test
        public static bool IsYes(string value)
        {
            if (BoolExtension.TryParseYesNo(value, out bool result))
            {
                return result;
            }            
            return false;
        }

        public static bool TryParseYesNo(string value, out bool result)
        {
            if (value.Equals(YesString, StringComparison.OrdinalIgnoreCase))
            {
                result = true;
                return true;
            }
            if(value.Equals(NoString, StringComparison.OrdinalIgnoreCase))
            {
                result = false;
                return true;
            }
            result = false;
            return false;
        }

        public static string ToYesNoString(this bool value)
        {
            return value ? YesString : NoString;
        }
    }
}
