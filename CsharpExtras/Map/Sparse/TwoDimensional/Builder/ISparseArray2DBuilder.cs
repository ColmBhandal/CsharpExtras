using System;

namespace CsharpExtras.Map.Sparse.TwoDimensional.Builder
{
    public interface ISparseArray2DBuilder<TVal>
    {
        ISparseArray2D<TVal> Build();
        ISparseArray2DBuilder<TVal> WithRowValidation(Func<int, bool> validator);
        ISparseArray2DBuilder<TVal> WithColumnValidation(Func<int, bool> validator);
        ISparseArray2DBuilder<TVal> WithValue(TVal value, int row, int column);
    }
}