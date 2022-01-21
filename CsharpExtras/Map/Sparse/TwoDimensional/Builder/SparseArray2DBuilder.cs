using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Map.Sparse.TwoDimensional.Builder
{
    internal class SparseArray2DBuilder<TVal> : ISparseArray2DBuilder<TVal>
    {
        public ISparseArray2D<TVal> Build()
        {
            //TODO
            return new SparseArray2DImpl<TVal>();
        }
    }
}
