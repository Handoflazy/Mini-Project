using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GameObject;

namespace Platformer.Factory
{
    public class ComponentFactory<T> : IFactory<T> where T : Component
    {
        public T Prefab { get; }

        public ComponentFactory(T prefab)
        {
            Prefab = prefab;
        }
        
        public T Create()
        {
            return GameObject.Instantiate(Prefab);
        }
    }
}