using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.EventChannel
{
    public abstract class EventListenter<T> : MonoBehaviour
    {
        [SerializeField] private EventChannel<T> eventChannel;
        [SerializeField] private UnityEvent<T> unityEvent;

        private void Awake()
        {
             eventChannel.Register(this);
        }

        public void Raise(T value)
        {
            unityEvent?.Invoke(value);
        }

        private void OnDestroy()
        {
            eventChannel.DeRegister(this);
        }
    }
}