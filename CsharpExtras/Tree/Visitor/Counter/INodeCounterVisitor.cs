namespace Tree.Visitor.Counter
{
    public interface INodeCounterVisitor<TPayload> : ITreeVisitor<TPayload>
    {
        int Count { get; }
    }
}