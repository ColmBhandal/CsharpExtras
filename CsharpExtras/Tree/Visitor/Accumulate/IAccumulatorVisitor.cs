namespace CsharpExtras.Tree.Visitor.Void.Integer
{
    public interface IAccumulatorVisitor<TResult>
    {
        TResult Result { get; }
    }
}