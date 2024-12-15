using System;
using Cysharp.Threading.Tasks;

namespace GameUsed.Core
{
    public delegate UniTask<PipeReturn> Pipe();

    public readonly struct PipeReturn
    {
        public bool      IsFaulty => Ex is not null;
        public bool      IsEnd    => Then is null;
        public Exception Ex       { get; }
        public Pipe  Then     { get; }

        public PipeReturn(Exception ex = null, Pipe then = null)
        {
            Ex   = ex;
            Then = then;
        }

        public static PipeReturn Except(Exception ex)
        {
            return new PipeReturn(ex);
        }

        public async UniTask<PipeReturn> Continue()
        {
            return await Then();
        }
    }
}