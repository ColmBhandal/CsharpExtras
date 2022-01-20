namespace CsharpExtras.Compare
{
    /// <summary>
    /// Encapsulates whether two objects are equal and also a message about the comparison result.
    /// </summary>
    public interface IComparisonResult
    {
        /// <summary>
        /// True iff the objects compared are equal
        /// </summary>
        bool IsEqual { get; }

        /// <summary>
        /// This provides more information about the comparison. E.g. it can help explain why objects are not equal.
        /// </summary>
        string Message { get; }
    }
}