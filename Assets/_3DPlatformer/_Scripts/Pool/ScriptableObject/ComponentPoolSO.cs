using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool
{
    
    /// <summary>
    /// Implements a Pool for Component types.
    /// </summary>
    /// <typeparam name="T">Specifies the component to pool.</typeparam
    public abstract class ComponentPoolSO<T>: PoolSO<T> where T: Component
    {
        private GameObject poolRootObject;
        public override IFactory<T> Factory { get; set; }

        private GameObject PoolRootObject
        {
            get
            {
                if (!Application.isPlaying)
                {
                    return null;
                }

                if (poolRootObject != null) return poolRootObject;
                poolRootObject = new GameObject(name);
                DontDestroyOnLoad(poolRootObject);

                return poolRootObject;
            }
        }

        public override T Request()
        {
            T member =  base.Request();
            member.gameObject.SetActive(true);
            return member;
        }

        public override void Return(T member)
        {
            member.transform.SetParent(PoolRootObject.transform);
            member.gameObject.SetActive(false);
            base.Return(member);
        }
        
        protected override T Create()
        {
            T newMember = base.Create();
            newMember.transform.SetParent(PoolRootObject.transform);
            return newMember;
        }
        
        public override void OnDisable()
        {
            base.OnDisable();
#if UNITY_EDITOR
            DestroyImmediate(PoolRootObject);
#else   
            Destroy(PoolRootObject);
#endif
        }
    }
}