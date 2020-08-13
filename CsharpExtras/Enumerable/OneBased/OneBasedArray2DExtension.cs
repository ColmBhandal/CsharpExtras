using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Enumerable.OneBased
{
    public static class OneBasedArray2DExtension
    {
        public static int LastUsedRow(this IOneBasedArray2D<string> array)
        {
            return array.LastUsedRow(s => !string.IsNullOrWhiteSpace(s));
        }
        public static int LastUsedColumn(this IOneBasedArray2D<string> array)
        {
            return array.LastUsedColumn(s => !string.IsNullOrWhiteSpace(s));
        }
    }
}
