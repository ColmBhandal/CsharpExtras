namespace Map
{

    /// <summary>
    /// A function map that should lazily cache param/result pairs until clear is called.
    /// </summary>    
    public interface ILazyFunctionMap<T, U> : IFunctionMap<T, U>
    {
        /// <summary>
        /// Should invalidate any caches forcing the underyling function to be called afresh
        /// </summary>
        void Clear();
    }
}