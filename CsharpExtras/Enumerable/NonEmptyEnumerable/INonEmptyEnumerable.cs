using System;
using System.Collections.Generic;

namespace NonEmptyEnumerable
{
    public interface INonEmptyEnumerable<T> : IEnumerable<T>
    {
        INonEmptyEnumerable<U> Map<U>(Func<T, U> func);
        int Count { get; }
    }
}