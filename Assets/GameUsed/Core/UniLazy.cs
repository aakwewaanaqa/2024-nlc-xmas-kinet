using System;
using Cysharp.Threading.Tasks;

namespace GameUsed.Core
{
    public class UniLazy<T>
    {
        private Func<UniTask<T>> factory        { get; }
        private T                value          { get; set; }
        public  bool             IsValueCreated { get; private set; }

        public UniTask<T> Value
        {
            get
            {
                if (IsValueCreated) return UniTask.FromResult(value);
                return UniTask.Create(async () =>
                {
                    value          = await factory();
                    IsValueCreated = true;
                    return value;
                });
            }
        }

        public UniLazy(Func<UniTask<T>> factory)
        {
            this.factory = factory;
        }
    }
}