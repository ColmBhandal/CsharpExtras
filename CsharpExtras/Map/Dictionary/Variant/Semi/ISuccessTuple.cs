namespace CsharpExtras.Map.Dictionary.Variant.Semi
{
    public interface ISuccessTuple<out V>
    {
        bool WasSuccessful { get; }
        V Value { get; }
    }

}
