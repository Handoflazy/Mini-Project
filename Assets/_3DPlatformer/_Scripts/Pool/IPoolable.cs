using System;

namespace Platformer.Pool
{
    /// <summary>
    /// Represents an object that can be pooled.
    /// </summary>
    public interface IPoolable
    {
        void Initialize();
        void Reset(Action onReset);
    }
}