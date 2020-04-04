using CsharpExtras.Log;
using IO;
using IO;
using OneBased;
using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Api
{
    public class CsharpExtrasApi : ICsharpExtrasApi
    {
        public IFileDecorator NewFileDecorator() => new FileDecoratorImpl(NewFileFacade());

        private IFileFacade NewFileFacade() => new FileFacadeImpl();

        public void SetLogger(ILogger logger)
        {
            StaticLogManager.Logger = logger;
        }

        public IOneBasedArray2D<TVal> NewOneBasedArray2D<TVal>(TVal[,] zeroBasedBackingArray)
        {
            return new OneBasedArray2DImpl<TVal>(zeroBasedBackingArray);
        }

        public IOneBasedArray<TVal> NewOneBasedArray<TVal>(TVal[] zeroBasedBackingArray)
        {
            return new OneBasedArrayImpl<TVal>(zeroBasedBackingArray);
        }
    }
}
