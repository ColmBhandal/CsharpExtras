namespace CsharpExtras.Map.Sparse.TwoDimensional.Builder
{
    internal interface ISparseArray2DBuilder<TVal>
    {
        ISparseArray2D<TVal> Build();
    }
}