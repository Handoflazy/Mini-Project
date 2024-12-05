using System.Collections.Generic;
using Platformer.Factory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Pool
{
    public abstract class PoolSO<T>: ScriptableObject, IPool<T> where T: IPoolable
    {
        private readonly Stack<T> available = new Stack<T>();
        public abstract IFactory<T> Factory { get; }
        
        [FormerlySerializedAs("_initialPoolSize")] [SerializeField]
        private int initialPoolSize = default;
        public virtual void OnEnable()
        {
            for (int i = 0; i < initialPoolSize; i++)
            {
                available.Push(Create());
            }
        }
        public virtual void OnDisable()
        {
            available.Clear();
        }
        public virtual T Create()
        {
            return Factory.Create();
        }
        public T Request()
        {
            if (available.Count <= 0)
            {
                available.Push(Create());
            }
            T member = available.Pop();
            member.Initialize();
            return member;
        }

        public IEnumerable<T> Request(int num)
        {
            List<T> members = new List<T>();
            for (int i = 0; i < num; i++)
            {
                members.Add(Request());
            }
            return members;
        }

        public void Return(T member)
        {
            member.Reset(() =>
            {
                available.Push(member);
            });
        }

        public void Return(IEnumerable<T> members)
        {
            foreach (T member in members)
            {
                Return(member);
            }
        }
    }
}