using System;
using System.Collections.Generic;
using Platformer.Pool.Example;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace Platformer.Systems.AudioSystem
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        [Header("Init Variables")]
        [Tooltip ("Amount of sound emitters created on Start")]
        public int initialPoolSize = 10;
        [Header ("Mixer Groups")]
        public AudioMixerGroup masterMixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup musicMixer;
        [SerializeField]
        private SoundEmitterPoolSO soundEmitterPool;
        [SerializeField]
        private GameObject soundEmittersContainer;
        private int poolIndex = 0;

        private void Start()
        {
            InitEmitterPool();

            float tempVal = Mathf.Infinity;

            Debug.Log(tempVal == Mathf.Infinity);
           
            
        }
        #region mixer groups functions
        public void ChangeVolumeOfMixerGroup(AudioMixerGroup mixer, float newVolumeNormalized)
        {
            mixer.audioMixer.SetFloat("Volume", NormalizedToMixerValue(newVolumeNormalized));
        }
        
        public void SaveVolumeOfMixerGroup(AudioMixerGroup mixer)
        {
            if (mixer.audioMixer.GetFloat("Volume", out var tempVolume))
            {
                PlayerPrefs.SetFloat(mixer.name, tempVolume);
            }
            else
                Debug.LogError("Could not save volume for mixer group " + mixer.name + ". It does not contain an exposer variable with name Volume" );
        }
        
        public void LoadVolumeOfMixerGroup(AudioMixerGroup mixer)
        {
            float tempVolume = PlayerPrefs.GetFloat(mixer.name, Mathf.Infinity);

            if (tempVolume != Mathf.Infinity)
            {
                mixer.audioMixer.SetFloat("Volume", tempVolume);
            }
            else
                Debug.Log("There is no saved volume preferences for mixer " + mixer.name + ", could not load volume.");
        }
        
        
        
        
        #region mixerHelpers
        // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations when using UI sliders normalized format
        private float MixerValueNormalized(float value)
        {
            return  (-(value - 80) / 80) - 1;
        }
        private float NormalizedToMixerValue(float normalizedValue)
        {
            return -80 + (normalizedValue * 80);
        }
        #endregion
        #endregion
        private void InitEmitterPool()
        {
            soundEmitterPool.Prewarm(initialPoolSize);
            soundEmitterPool.SetParent(soundEmittersContainer.transform);
            soundEmitterPool.name = "Sound Emitters Pool";
        }
        public SoundEmitter Play2DSound(AudioClip clip, AudioMixerGroup mixer, bool loop = false, float volume = 1, float pitch = 1)
        {
            if (!mixer)
                mixer = masterMixer;

            SoundEmitter foundSoundEmit = soundEmitterPool.Request();

            foundSoundEmit.PlaySound(clip, mixer, Vector3.zero,false, loop, volume, pitch);
            soundEmitterPool.Return(foundSoundEmit);

            return foundSoundEmit;
        }
        
        public SoundEmitter PlaySpatialSound(AudioClip clip, AudioMixerGroup mixer, Vector3 position, bool loop = false, float volume = 1, float pitch = 1)
        {
            if (!mixer)
                mixer = masterMixer;

            SoundEmitter foundSoundEmit = soundEmitterPool.Request();

            foundSoundEmit.PlaySound(clip, mixer, position,true, loop, volume, pitch);
            soundEmitterPool.Return(foundSoundEmit);
            return foundSoundEmit;
        }
        public void StopAllSounds()
        {
            var soundEmitters = soundEmitterPool.Request(initialPoolSize);
            foreach (SoundEmitter emit in soundEmitters)
            {
                if (emit.IsInUse())
                    emit.StopSound();
            }
        }

        public void StopAllLoopSound()
        {
            var soundEmitters = soundEmitterPool.GetAvailable();
            foreach (SoundEmitter emit in soundEmitters)
            {
                if (emit.IsInUse())
                    emit.StopSound();
            }
        }
        
        
    }
}