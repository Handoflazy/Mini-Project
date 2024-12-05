using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool
{
    public abstract class ComponentPoolSO<T>: PoolSO<T> where T: Component, IPoolable
    {
        [SerializeField]
        private string name = default;
        
        private GameObject poolRootObject;
        public override void OnEnable()
        {
            if (poolRootObject == null)
            {
                poolRootObject = new GameObject(name);
            }
            base.OnEnable();
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