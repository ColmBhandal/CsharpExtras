using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpExtras.Event.Notify
{
    internal class UpdateNotifierImpl<TVal, TUpdate> : IUpdateNotifier<TVal, TUpdate>
    {
        public event Action<TUpdate>? BeforeUpdate;
        public event Action<TUpdate>? AfterUpdate;

        public TVal Value { get; private set; }
        private readonly Func<TVal, TUpdate, TVal> _updateFunction;

        public UpdateNotifierImpl(TVal value, Func<TVal, TUpdate, TVal> updateFunction)
        {
            Value = value;
            _updateFunction = updateFunction ?? throw new ArgumentNullException(nameof(updateFunction));
        }

        public void Update(TUpdate update)
        {
            BeforeUpdate?.Invoke(update);
            Value = _updateFunction(Value, update);
            AfterUpdate?.Invoke(update);
        }
    }

}
