using System;
using System.Collections;
using System.Collections.Generic;
using Platformer.Pool.Example;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using Random = Unity.Mathematics.Random;

namespace Platformer.Systems.AudioSystem
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        [Header("Init Variables")]
        [Tooltip ("Amount of sound emitters created on Start")]
        public int initialPoolSize = 10;

        [SerializeField] private SoundEmitter prefab;
        
        
        
        [Header ("Mixer Groups")]
        public AudioMixerGroup masterMixer;
        public AudioMixerGroup sfxMixer;
        public AudioMixerGroup musicMixer;
        [SerializeField]
        private SoundEmitterPoolSO soundEmitterPool;
        [SerializeField]
        private GameObject soundEmittersContainer;

        private void Start()
        {
            InitEmitterPool();
            
        }
        #region mixer groups functions
        public static bool SetGroupVolume(AudioMixerGroup mixer, float newVolumeNormalized)
        {
           return mixer.audioMixer.SetFloat("Volume", NormalizedToMixerValue(newVolumeNormalized));
        }

        public static bool GetGroupVolume(AudioMixerGroup group, out float volume)
        {
            if(group.audioMixer.GetFloat("Volume", out float rawVolume)){
                volume = MixerValueNormalized(rawVolume);
                return true;
            }
            volume = default;
            return false;
        }
        
        
        #region mixerHelpers
        // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations when using UI sliders normalized format
        private static float MixerValueNormalized(float value)
        {
            return  (-(value - 80) / 80) - 1;
        }
        private static float NormalizedToMixerValue(float normalizedValue)
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
        public SoundEmitter PlaySound(AudioClip clip, AudioConfigurationSO settings, Vector3 position = default)
        {

            SoundEmitter foundSoundEmit = soundEmitterPool.Request();
            if (foundSoundEmit)
            {
                //foundSoundEmit.PlaySound(clip, settings,position);
                StartCoroutine(WaitForClipEnd(foundSoundEmit, clip));
            }
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
        private IEnumerator WaitForClipEnd(SoundEmitter emitter, AudioClip clip)
        {
            yield return new WaitForSeconds(clip.length);
            soundEmitterPool.Return(emitter);
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