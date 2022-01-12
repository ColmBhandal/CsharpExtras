namespace CsharpExtras.Extensions.Helper.Dictionary
{
    public interface IDictionaryComparison
    {
        bool IsEqual { get; }
        string Message { get; }
    }
}