using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonEmptyEnumerable.Impl
{
    public class NonEmptyEnumerableImpl<T> : INonEmptyEnumerable<T>
    {
        IEnumerable<T> _delegate;

        public NonEmptyEnumerableImpl(IEnumerable<T> del)
        {
            if (del == null || del.Count() == 0)
            {
                throw new ArgumentException("Must provide at least one element for non-empty collection");
            }
            _delegate = del;
        }

        public NonEmptyEnumerableImpl(T singletonValue)
        {
            _delegate = new HashSet<T> { singletonValue };
        }

        public int Count => _delegate.Count();

        public IEnumerator<T> GetEnumerator()
        {
            return _delegate.GetEnumerator();
        }

        public INonEmptyEnumerable<U> Map<U>(Func<T, U> func)
        {
            return new NonEmptyEnumerableImpl<U>(this.Select(func));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _delegate.GetEnumerator();
        }
    }
}
