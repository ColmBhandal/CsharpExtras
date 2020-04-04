namespace CsharpExtras.Visitor.Result
{
    public interface IResultStorageVisitorBase<TVisitable, TResult> : IVisitorBase<TVisitable> where TVisitable : IVisitableBase<TVisitable>
    {
        TResult Result { get; }
    }
}