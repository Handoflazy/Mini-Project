using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Event_System.EventChannel
{
    public abstract class EventChannel<T> : ScriptableObject
    {
        private readonly HashSet<EventListenter<T>> observers = new();

        public void Invoke(T value)
        {
            foreach (var observer in observers)
            {
                observer.Raise(value);
            }
        }

        public void Register(EventListenter<T> observer) => observers.Add(observer);
        public void DeRegister(EventListenter<T> observer) => observers.Remove(observer);
    }

    public readonly struct Empty{}
    
    [CreateAssetMenu(menuName = "Events/Empty Channel")]
    public class EmptyEventChannel : EventChannel<Empty>
    {
        
    }
}
