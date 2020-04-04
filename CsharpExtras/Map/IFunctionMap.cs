namespace CsharpExtras.Map
{
    public interface IFunctionMap<T, U>
    {
        /// <summary>
        /// Mapped values representing the underlying function.
        /// </summary>
        U this[T index] { get; }
    }
}