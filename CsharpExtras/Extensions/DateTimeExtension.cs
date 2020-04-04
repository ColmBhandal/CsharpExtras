using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.CustomExtensions
{
    public static class DateTimeExtension
    {
        public static string GetShortDateStamp(this DateTime dateTime)
        {
            return dateTime.ToString("yyMMdd");
        }            
    }
}
