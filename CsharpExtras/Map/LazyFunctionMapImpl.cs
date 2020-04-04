using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Map
{
    /// <summary>
    /// The lazy function map caches the results of calls to the backing function. Every time the function is called on an argument u and returns v,
    /// the pair (u, v) is stored in the dictionary.
    /// </summary>
    class LazyFunctionMapImpl<T, U> : ILazyFunctionMap<T, U>
    {
        private readonly Dictionary<T, U> _backingDictionary;
        private readonly Func<T, U> _backingFunction;

        public LazyFunctionMapImpl(Func<T, U> backingFunction)
        {
            _backingDictionary = new Dictionary<T, U>();
            _backingFunction = backingFunction;
        }

        public U this[T index]
        {
            get
            {
                if (_backingDictionary.ContainsKey(index))
                {                    
                    return _backingDictionary[index];
                }                
                U valueAtIndex = _backingFunction(index);
                _backingDictionary.Add(index, valueAtIndex);
                return valueAtIndex;
            }
        }

        public void Clear()
        {
            _backingDictionary.Clear();
        }
    }
}
