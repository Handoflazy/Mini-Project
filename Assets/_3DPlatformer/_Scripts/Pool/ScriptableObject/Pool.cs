using System.Collections.Generic;
using Platformer.Factory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Pool
{
    
    /// <summary>
    /// A generic pool that generates members of type T on-demand via a factory.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements to pool.</typeparam>
    public abstract class Pool<T>: ScriptableObject, IPool<T> where T: IPoolable
    {
        protected readonly Stack<T> available = new Stack<T>();
        public abstract IFactory<T> Factory { get; set; }
        [SerializeField]
        private int initialPoolSize = default;
        public virtual void OnDisable()
        {
            available.Clear();
        }
        protected virtual T Create() => Factory.Create();

       

        public virtual T Request()
        {
            T member = available.Count > 0 ? available.Pop() : Create();
            member.OnRequest();
            return member;
        }

        public virtual IEnumerable<T> Request(int num = 1)
        {
            List<T> members = new List<T>(num);
            for (int i = 0; i < num; i++)
            {
                members.Add(Request());
            }
            return members;
        }

        public virtual void Return(T member)
        {
            member.OnReturn(() =>
            {
                available.Push(member);
            });
        }

        public virtual void Return(IEnumerable<T> members)
        {
            foreach (T member in members)
            {
                Return(member);
            }
        }
    }
}