namespace CsharpExtras.Map.Sparse.TwoDimensional
{
    public interface ISparseArray2D<TVal>
    {
        void Set(TVal[,] area, int rowIndex, int colIndex);
    }
}