using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpExtras.Tree.Base
{    
    class LeafBase<TPayload> : PayloadContainerTreeBase<TPayload>, ILeafBase<TPayload>
    {
        public LeafBase(TPayload payload) : base(payload)
        {
        }

        public override IEnumerable<ITreeBase<TPayload>> Children => new HashSet<ITreeBase<TPayload>>();
    }
    
    public interface ILeafBase<TPayload> : ITreeBase<TPayload>
    {

    }
}
