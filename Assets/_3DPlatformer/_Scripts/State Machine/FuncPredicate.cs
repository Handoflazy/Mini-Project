using System;

namespace Platformer.State_Machine
{
    public class FuncPredicate: IPredicate
    {
        readonly Func<bool> _func;

        public FuncPredicate(Func<bool> func)
        {
            _func = func;
        }
        public bool Evaluate() => _func.Invoke();
    }
}