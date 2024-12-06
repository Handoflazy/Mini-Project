using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Event_System.EventChannel;

namespace Platformer.Systems.AudioSystem
{
    public class AudioCue: MonoBehaviour
    {
        [SerializeField] private AudioCueSO _audioCue = default;
        [SerializeField] private AudioConfigurationSO _audioConfiguration = default;
        [Header("Broadcast On")]
        [SerializeField] private AudioCueChannelSO _audioCueEventChannel = default;
        [Button]
        public void PlayAudioCue()
        {
            _audioCueEventChannel.Invoke(_audioCue, _audioConfiguration, transform.position);
        }
    }

  
}