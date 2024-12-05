using UnityEngine;

namespace Platformer.Factory
{
    public abstract class ComponentFactorySO<T> : ScriptableObject, IFactory<T> where T: Component
    {
        public abstract T Prefab { get; }
        public T Create()
        {
            return Instantiate(Prefab);
        }
    }
}