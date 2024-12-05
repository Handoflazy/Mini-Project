using UnityEngine;

namespace Platformer.Factory
{

    public class Factory<T> : ScriptableObject, IFactory<T> where T : new()

    {
        public T Create()
        {
            return new T();
        }
    }
}