using Platformer.Systems.AudioSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities.Event_System.EventChannel
{
    public class AudioCueListener : MonoBehaviour
    {
        [SerializeField] private AudioCueChannelSO eventChannel;
        [SerializeField] private UnityEvent<AudioCueSO, AudioConfigurationSO , Vector3> unityEvent;

        private void Awake()
        {
            eventChannel.Register(this);
        }

        public void Raise(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace)
        {
            unityEvent?.Invoke(audioCue,audioConfiguration,positionInSpace);
        }

        private void OnDestroy()
        {
            eventChannel.DeRegister(this);
        }
    }
}