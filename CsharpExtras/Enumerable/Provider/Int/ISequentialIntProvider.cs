namespace CsharpExtras.Enumerable.Provider.Int
{
    /// <summary>
    /// Provides iteration over an arithmetic sequence of ints, starting at the given value & increasing by the given step
    /// </summary>
    public interface ISequentialIntProvider
    {
        /// <summary>
        /// Value to start at
        /// </summary>
        int Start { get; }

        /// <summary>
        /// Value by which to increase the sequence at each successive term
        /// </summary>
        int Step { get; }
        
        /// <returns>The next successive term in the sequence</returns>
        int Next();
    }
}