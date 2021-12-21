namespace CsharpExtras.Enumerable.Provider.Int
{
    public interface ISequentialIntProvider
    {
        int Start { get; }
        int Step { get; }

        int Next();
    }
}