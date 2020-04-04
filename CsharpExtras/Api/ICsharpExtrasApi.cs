using CsharpExtras.Log;
using IO;
using OneBased;

namespace CsharpExtras.Api
{
    public interface ICsharpExtrasApi
    {
        void SetLogger(ILogger logger);
        IFileDecorator NewFileDecorator();
        IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(TVal[,] zeroBasedBackingArray);
        IOneBasedArray<TVal> NewOneBasedArray<TVal>(TVal[] zeroBasedBackingArray);
    }
}