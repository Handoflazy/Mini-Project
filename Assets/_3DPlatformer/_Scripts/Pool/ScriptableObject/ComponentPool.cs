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
        
        private void InitializePool()
        {
            poolRootObject = new GameObject(name);
            DontDestroyOnLoad(poolRootObject);
            for (int i = 0; i < InitialPoolSize; i++)
            {
                available.Push(Create());
            }
        }
        public override T Request()
        {
            if (poolRootObject == null)
            {
                InitializePool();
            }
            return base.Request();
        }

        public override void Return(T member)
        {
            if(poolRootObject==null)
                InitializePool();
            base.Return(member);
        }
        
        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(poolRootObject.transform);
            return newMember;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
#if UNITY_EDITOR
            DestroyImmediate(poolRootObject);
#else   
            Destroy(_poolRootObject);
#endif
        }
    }
}