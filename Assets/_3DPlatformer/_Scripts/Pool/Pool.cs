using System.Collections;
using System.Collections.Generic;
using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool
{
    /// <summary>
    /// A generic pool that generates members of type T on-demand via a factory.
    /// </summary>
    /// <typeparam name="T">Specifies the type of elements in the pool.</typeparam>
    public class Pool<T>: IPool<T> where T: IPoolable
    {
        public Stack<T> available = new Stack<T>();
        IFactory<T> _factory;
        public Pool(IFactory<T> factory) : this(factory, 0) { }
        public Pool(IFactory<T> factory, int initialPoolSize)
        {
            this._factory = factory;

            for (int i = 0; i < initialPoolSize; i++)
            {
                available.Push(_factory.Create());
            }
        }

        public T Request()
        {
            if (available.Count <= 0)
            {
                available.Push(_factory.Create());
            }
            T member = available.Pop();
            member.Initialize();
            return member;
        }
        public IEnumerable<T> Request(int num=1)
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
            foreach(T member in members)
            {
                Return(member);
            }
        }
    }
}