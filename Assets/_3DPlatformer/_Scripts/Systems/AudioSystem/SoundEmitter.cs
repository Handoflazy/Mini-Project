using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Platformer.Systems.AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource audioSource;


        private void Awake()
        {
            DontDestroyOnLoad(this);
            audioSource = this.GetOrAddComponent<AudioSource>();
        }

        private void Start()
        {
            audioSource.playOnAwake = false;
        }

        public void PlaySound(AudioClip clip, AudioMixerGroup mixer,Vector3 position, bool isPartialSound,
            bool loop, float volume, float pitch = 1)
        {
            if (!isPartialSound)
            {
                audioSource.spatialBlend = 0;
            }
            else
            {
                audioSource.spatialBlend = 1;
                this.transform.position = position;
            }
            audioSource.clip = clip;
            audioSource.outputAudioMixerGroup = mixer;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.pitch = pitch;

            audioSource.Play();
        }

        public void StopSound()
        {
            audioSource.Stop();
        }

        public bool IsInUse()
        {
            return audioSource.isPlaying;
        }

        public bool isLooping()
        {
            return audioSource.loop;
        }
    }
}