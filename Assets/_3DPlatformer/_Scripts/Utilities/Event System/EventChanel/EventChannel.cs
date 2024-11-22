using System.Collections.Generic;
using Platformer;

namespace Utilities.Event_System.EventChannel
{
    public abstract class EventChannel<T> : DescriptionBaseSO
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
}
