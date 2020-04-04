using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree
{
    abstract class ChildSetTreeBase<TPayload, TRec, TLeaf> : PayloadContainerTreeBase<TPayload>, IChildSetTree<TPayload, TRec>
        where TLeaf : ITreeBase<TPayload> where TRec : IChildSetTree<TPayload, TRec>
    {
        private readonly ISet<ITreeBase<TPayload>> _chilren = new HashSet<ITreeBase<TPayload>>();

        public ChildSetTreeBase(TPayload payload) : base(payload)
        {
        }

        public override IEnumerable<ITreeBase<TPayload>> Children => _chilren;

        public TRec WithChild(ITreeBase<TPayload> child)
        {
            _chilren.Add(child);
            return GetMe();
        }

        public TRec WithLeaf(TPayload childPaylod)
        {
            TLeaf child = LeafFromPayload(childPaylod);
            return WithChild(child);
        }

        protected abstract TRec GetMe();
        protected abstract TLeaf LeafFromPayload(TPayload payload);
    }
}
