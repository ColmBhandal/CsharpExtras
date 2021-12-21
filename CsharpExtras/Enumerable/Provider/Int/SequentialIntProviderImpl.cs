using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Enumerable.Provider.Int
{
    internal class SequentialIntProviderImpl : ISequentialIntProvider
    {
        public int Start { get; }
        public int Step { get; }

        public SequentialIntProviderImpl(int start, int step)
        {
            Start = start;
            Step = step;
        }

        public int Next()
        {
            return 1;
        }
    }
}
