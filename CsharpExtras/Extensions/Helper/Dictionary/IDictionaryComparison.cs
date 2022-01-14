namespace CsharpExtras.Extensions.Helper.Dictionary
{
    /// <summary>
    /// A dictionary comparison encapsulates whether dictionaries are equal and also a message about the comparison result.
    /// </summary>
    public interface IDictionaryComparison
    {
        /// <summary>
        /// True iff the dictionaries that were compared are equal
        /// </summary>
        bool IsEqual { get; }

        /// <summary>
        /// This provides more information about the comparison. E.g. if the dictionaries are not equal, it can help explain why.
        /// </summary>
        string Message { get; }
    }
}