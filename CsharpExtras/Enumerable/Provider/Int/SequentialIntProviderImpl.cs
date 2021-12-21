using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Enumerable.Provider.Int
{
    internal class SequentialIntProviderImpl : ISequentialIntProvider
    {
        public int Start { get; }
        public int Step { get; }

        private int _current;

        public SequentialIntProviderImpl(int start, int step)
        {
            Start = start;
            Step = step;
            _current = start;
        }

        public int Next()
        {
            AssertNextIsInBounds();
            _current += Step;
            return _current;
        }

        private void AssertNextIsInBounds()
        {
            AssertNextLeqIntMax();
            AssertNextGeqIntMin();
        }

        private void AssertNextGeqIntMin()
        {
            if (Step < 0)
            {
                int MinAllowableCurrent = int.MinValue - Step;
                if (_current < MinAllowableCurrent)
                {
                    throw new IndexOutOfRangeException($"Current index {_current} plus step {Step} will exceed int Min value");
                }
            }
        }

        private void AssertNextLeqIntMax()
        {
            if(Step > 0)
            {
                int maxAllowableCurrent = int.MaxValue - Step;
                if(_current > maxAllowableCurrent)
                {
                    throw new IndexOutOfRangeException($"Current index {_current} plus step {Step} will exceed int Max value");
                }
            }
        }
    }
}
