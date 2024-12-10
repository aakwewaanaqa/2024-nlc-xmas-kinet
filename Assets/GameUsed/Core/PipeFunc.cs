using System;
using Cysharp.Threading.Tasks;

namespace GameUsed.Core
{
    public delegate UniTask<PipeReturn> PipeFunc();

    public readonly struct PipeReturn
    {
        public bool      IsFaulty => Ex is not null;
        public bool      IsEnd    => Then is null;
        public Exception Ex       { get; }
        public PipeFunc  Then     { get; }

        public PipeReturn(Exception ex = null, PipeFunc then = null)
        {
            Ex   = ex;
            Then = then;
        }

        public async UniTask<PipeReturn> Continue()
        {
            return await Then();
        }
    }
}