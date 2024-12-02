using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Event_System.EventChannel
{
    public class VoidListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannel eventChannel;
        [SerializeField] private UnityEvent unityEvent;

        private void Awake()
        {
            eventChannel.Register(this);
        }
        public void Raise()
        {
            unityEvent?.Invoke();
        }
        
        private void OnDestroy()
        {
            print("disable"+ gameObject.name);
            eventChannel.DeRegister(this);
        }
    }
}