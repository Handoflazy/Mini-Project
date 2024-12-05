using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool
{
    
    /// <summary>
    /// Implements a Pool for Component types.
    /// </summary>
    /// <typeparam name="T">Specifies the component to pool.</typeparam
    public abstract class ComponentPool<T>: Pool<T> where T: Component, IPoolable
    {
        public abstract int InitialPoolSize { get; set; }
        private GameObject poolRootObject;
        public override T Request()
        {
            if (poolRootObject == null)
            {
                poolRootObject = new GameObject(name);
                DontDestroyOnLoad(poolRootObject);
                for (int i = 0; i < InitialPoolSize; i++)
                {
                    available.Push(Add());
                }
            }
            return base.Request();
        }

        public override T Add()
        {
            T newMember = base.Add();
            newMember.transform.SetParent(poolRootObject.transform);
            return newMember;
        }

        public override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(poolRootObject.transform);
            return newMember;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
            DestroyImmediate(poolRootObject);
        }
    }
}