
using System.Collections;
using Platformer.Pool.Example;
using UnityEngine;
using UnityEngine.Audio;
using Utilities.Event_System.EventChannel;

namespace Platformer.Systems.AudioSystem
{
    [RequireComponent(typeof(AudioCueListener))]
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters pool")]
        [SerializeField] private SoundEmitterPoolSO pool;
        [SerializeField] int initialPoolSize = 10;
        [SerializeField] private SoundEmitter prefab;
        [SerializeField] private GameObject soundEmittersContainer;

        [Header("Audio control")] // througt settingsUI
        [SerializeField] private AudioMixer audioMixer = default;
        [Range(0f, 1f)]
        [SerializeField] private float masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume = 1f;
        public SoundEmitterPoolSO Pool
        {
            get => Pool;
        }

        private void Awake()
        {
            InitPool();
        }
        
        #region mixer groups functions
        public static bool SetGroupVolume(AudioMixerGroup group, float volume)
        {
           return group.audioMixer.SetFloat("Volume", NormalizedToMixerValue(volume));
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
        private void InitPool()
        {
            pool.Prewarm(initialPoolSize);
            pool.SetParent(this.transform);
        }
        public void PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
        {
            AudioClip[] clipsToPlay = audioCue.GetClips();
            for (int i = 0; i < clipsToPlay.Length; i++)
            {
                SoundEmitter foundSoundEmit = pool.Request();
                if (foundSoundEmit)
                {
                    foundSoundEmit.PlaySound(clipsToPlay[i], settings,audioCue.looping,position);
                    if(!audioCue.looping)
                        StartCoroutine(WaitForClipEnd(foundSoundEmit, clipsToPlay[i]));
                }
                
            }
        }
        private IEnumerator WaitForClipEnd(SoundEmitter emitter, AudioClip clip)
        {
            yield return new WaitForSeconds(clip.length);
            pool.Return(emitter);
        }
        
        
    }
}