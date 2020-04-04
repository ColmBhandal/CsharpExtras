using Visitor;

namespace Tree.Visitor
{
    public interface ITreeVisitor<TPayload> : IVisitorBase<ITreeBase<TPayload>>
    {
    }
}