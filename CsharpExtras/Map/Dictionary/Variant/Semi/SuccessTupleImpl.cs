namespace CsharpExtras.Map.Dictionary.Variant.Semi
{
    public class SuccessTupleImpl<V> : ISuccessTuple<V>
    {
        public bool WasSuccessful { get; }
        public V Value {get;}

        public SuccessTupleImpl(bool wasSuccessful, V value)
        {
            WasSuccessful = wasSuccessful;
            Value = value;
        }
    }

}
