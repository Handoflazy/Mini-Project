using UnityEngine;

namespace Platformer.Factory
{

    public class FactorySO<T> : ScriptableObject, IFactory<T> where T : new()

    {
        public T Create()
        {
            return new T();
        }
    }
}