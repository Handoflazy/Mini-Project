using System.Collections.Generic;
using Platformer.Systems.AudioSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Event_System.EventChannel
{
    [CreateAssetMenu(menuName = "Events/AudioCue Channel")]
    public class AudioCueChannelSO : ScriptableObject
    {
        private readonly HashSet<AudioCueListener> observers = new();

        public void Invoke(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
        {
            foreach (var observer in observers)
            {
                observer.Raise(audioCue,audioConfiguration,positionInSpace);
            }
        }

        public void Register(AudioCueListener observer) => observers.Add(observer);
        public void DeRegister(AudioCueListener observer) => observers.Remove(observer);
    }
    
}