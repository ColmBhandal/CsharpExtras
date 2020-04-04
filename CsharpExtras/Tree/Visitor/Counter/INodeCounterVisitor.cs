namespace CsharpExtras.Tree.Visitor.Counter
{
    public interface INodeCounterVisitor<TPayload> : ITreeVisitor<TPayload>
    {
        int Count { get; }
    }
}