using Platformer.Systems.AudioSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Event_System.EventChannel
{
   
    public class AudioCueChannelSO
    {
        public UnityAction<AudioCueSO, AudioConfigurationSO, Vector3> eventRaised;
        
        public void Raise(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
        {
            eventRaised.Invoke(audioCue, audioConfiguration, positionInSpace);
        }
    }
}