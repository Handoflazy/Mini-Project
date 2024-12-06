using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Platformer.Systems.AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        private AudioSource audioSource;
        private float _lastUseTimestamp = 0;
        public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            audioSource = this.GetOrAddComponent<AudioSource>();
        }

        private void Start()
        {
            audioSource.playOnAwake = false;
        }

        public void PlaySound(AudioClip clip,AudioConfigurationSO settings,bool hasToLoop, Vector3 position = default)
        {
            audioSource.clip = clip;
            ApplySettings(audioSource, settings);
            this.transform.position = position;
            audioSource.loop = hasToLoop;

            audioSource.Play();
        }

        private void ApplySettings(AudioSource source, AudioConfigurationSO settings)
        {
            source.outputAudioMixerGroup = settings.OutputAudioMixerGroup;
            source.mute = settings.Mute;
            source.bypassEffects = settings.BypassEffects;
            source.bypassListenerEffects = settings.BypassListenerEffects;
            source.bypassReverbZones = settings.BypassReverbZones;
            source.priority = settings.Priority;
            source.volume = settings.Volume;
            source.pitch = settings.Pitch;
            source.panStereo = settings.PanStereo;
            source.spatialBlend = settings.SpatialBlend;
            source.reverbZoneMix = settings.ReverbZoneMix;
            source.dopplerLevel = settings.DopplerLevel;
            source.spread = settings.Spread;
            source.rolloffMode = settings.RolloffMode;
            source.minDistance = settings.MinDistance;
            source.maxDistance = settings.MaxDistance;
            source.ignoreListenerVolume = settings.IgnoreListenerVolume;
            source.ignoreListenerPause = settings.IgnoreListenerPause;
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