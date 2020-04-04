using CsharpExtras.Visitor;

namespace CsharpExtras.Tree.Visitor
{
    public interface ITreeVisitor<TPayload> : IVisitorBase<ITreeBase<TPayload>>
    {
    }
}