namespace CsharpExtras.Visitor
{
    public interface IVisitableBase<TVisitable> where TVisitable : IVisitableBase<TVisitable>
    {
        void Accept(IVisitorBase<TVisitable> visitor);
    }
}