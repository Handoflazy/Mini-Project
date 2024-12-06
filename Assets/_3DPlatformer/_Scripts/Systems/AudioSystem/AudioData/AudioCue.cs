using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Event_System.EventChannel;

namespace Platformer.Systems.AudioSystem
{
    public class AudioCue: MonoBehaviour
    {
        [Header("Sound definition")]
        [SerializeField] private AudioCueSO _audioCue = default;

        [SerializeField] private bool playOnStart; 
        [Header("Configuration")]
        [SerializeField] private AudioConfigurationSO _audioConfiguration = default;
        [Header("Broadcast On")]
        [SerializeField] private AudioCueChannelSO _audioCueEventChannel = default;
        [Button]
        public void PlayAudioCue()
        {
            _audioCueEventChannel.Invoke(_audioCue, _audioConfiguration, transform.position);
        }

        private void Start()
        {
            if (playOnStart)
                StartCoroutine(PlayDelayed());
        }

        private IEnumerator PlayDelayed()
        {
            //The wait allows the AudioManager to be ready for play requests
            yield return new WaitForSeconds(1f);

            //This additional check prevents the AudioCue from playing if the object is disabled or the scene unloaded
            //This prevents playing a looping AudioCue which then would be never stopped
            if (playOnStart)
                PlayAudioCue();
        }
        
    }

  
}