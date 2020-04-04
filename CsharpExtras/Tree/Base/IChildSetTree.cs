using System.Collections.Generic;

namespace Tree
{
    interface IChildSetTree<TPayload, TRec> : ITreeBase<TPayload> where TRec : ITreeBase<TPayload>
    {
        TRec WithChild(ITreeBase<TPayload> child);
        TRec WithLeaf(TPayload childPaylod);
    }
}