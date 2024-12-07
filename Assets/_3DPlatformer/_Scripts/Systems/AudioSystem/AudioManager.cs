
using System;
using System.Collections;
using Platformer.Pool.Example;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using Utilities.Event_System.EventChannel;

namespace Platformer.Systems.AudioSystem
{
    public class AudioManager : MonoBehaviour
    {
        [Header("SoundEmitters pool")]
        [SerializeField] private SoundEmitterPoolSO pool;
        [SerializeField] int initialPoolSize = 10;
        [SerializeField] private SoundEmitter prefab;
        [SerializeField] private GameObject soundEmittersContainer;
        
        
        [Header("Listening on channels")]
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play SFXs")]
        [SerializeField] private AudioCueEventChannelSO SFXEventChannel = default;
        [Tooltip("The SoundManager listens to this event, fired by objects in any scene, to play Music")]
        [SerializeField] private AudioCueEventChannelSO musicEventChannel = default;   

        [Header("Audio control")]  
        [SerializeField] private AudioMixer audioMixer = default;
        [Range(0f, 1f)]
        [SerializeField] private float masterVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume = 1f;
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume = 1f;

        private SoundEmitterVault soundEmitterVault;
        private SoundEmitter musicSoundEmitter;
        private void Awake()
        {
            soundEmitterVault = new SoundEmitterVault();
            InitPool();
        }

        private void OnEnable()
        {
            SFXEventChannel.OnAudioCuePlayRequested += PlayAudioCue;
            SFXEventChannel.OnAudioCueStopRequested += StopAudioCue;
            SFXEventChannel.OnAudioCueFinishRequested += FinishAudioCue;

            musicEventChannel.OnAudioCuePlayRequested += PlayMusicTrack;
            musicEventChannel.OnAudioCueStopRequested += StopMusic;
            
        }

        private bool StopMusic(AudioCueKey emitterkey)
        {
            if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
            {
                musicSoundEmitter.Stop();
                return true;
            }
            else
                return false;
        }
        public void TimelineInterruptsMusic()
        {
            StopMusic(AudioCueKey.Invalid);
        }

        private bool FinishAudioCue(AudioCueKey audioCueKey)
        {
            bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);
            if (isFound)
            {
                for (int i = 0; i < soundEmitters.Length; i++)
                {
                    soundEmitters[i].Finish();
                    soundEmitters[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
            }
            else
            {
                Debug.LogWarning("Finishing an AudioCue was requested, but the AudioCue was not found.");
            }
            return isFound;
        }

        private bool StopAudioCue(AudioCueKey audioCueKey)
        {
            bool isFound = soundEmitterVault.Get(audioCueKey, out SoundEmitter[] soundEmitters);

            if (isFound)
            {
                for (int i = 0; i < soundEmitters.Length; i++)
                {
                    StopAndCleanEmitter(soundEmitters[i]);
                }

                soundEmitterVault.Remove(audioCueKey);
            }

            return isFound;
        }

        private void StopAndCleanEmitter(SoundEmitter soundEmitter)
        {
            if (!soundEmitter.IsLooping())
                soundEmitter.OnSoundFinishedPlaying -= OnSoundEmitterFinishedPlaying;
            soundEmitter.Stop();
            pool.Return(soundEmitter);
            
            //TODO: is the above enough?
            //_soundEmitterVault.Remove(audioCueKey); is never called if StopAndClean is called after a Finish event
            //How is the key removed from the vault?
        }

        #region mixer groups functions
        
        public void ChangeMasterVolume(float newVolume)
        {
            masterVolume = newVolume;
            SetGroupVolume("MasterVolume", masterVolume);
        }
        public void ChangeMusicVolume(float newVolume)
        {
            musicVolume = newVolume;
            SetGroupVolume("MusicVolume", musicVolume);
        }
        public void ChangeSFXVolume(float newVolume)
        {
            sfxVolume = newVolume;
            SetGroupVolume("SFXVolume", sfxVolume);
        }
        public void SetGroupVolume(string parameterName, float volume)
        {
            bool volumeSet = audioMixer.SetFloat(parameterName, NormalizedToMixerValue(volume));
            if (!volumeSet)
                Debug.LogError("The AudioMixer parameter was not found");
        }

        public float GetGroupVolume(string parameterName)
        {
            if (audioMixer.GetFloat(parameterName, out float rawVolume))
            {
                return MixerValueToNormalized(rawVolume);
            }
            else
            {
                Debug.LogError("The AudioMixer parameter was not found");
                return 0f;
            }
        }
        
        
        #region mixerHelpers
        // Both MixerValueNormalized and NormalizedToMixerValue functions are used for easier transformations when using UI sliders normalized format
        private  float MixerValueToNormalized(float value)
        {
            return  (-(value - 80) / 80) - 1;
        }
        private  float NormalizedToMixerValue(float normalizedValue)
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
        public AudioCueKey PlayAudioCue(AudioCueSO audioCue, AudioConfigurationSO settings, Vector3 position = default)
        {
            AudioClip[] clipsToPlay = audioCue.GetClips();
            SoundEmitter[] soundEmitterArray = pool.Request(clipsToPlay.Length) as SoundEmitter[];
            int nOfClips = clipsToPlay.Length;
            for (int i = 0; i < nOfClips; i++)
            {
                if (soundEmitterArray[i])
                {
                    soundEmitterArray[i].PlayAudioClip(clipsToPlay[i], settings,audioCue.looping,position);
                    if(!audioCue.looping)
                        soundEmitterArray[i].OnSoundFinishedPlaying += OnSoundEmitterFinishedPlaying;
                }
                
            }
            return soundEmitterVault.Add(audioCue, soundEmitterArray);
        }

        public AudioCueKey PlayMusicTrack(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration,
            Vector3 positionInSpace)
        {
            float fadeDuration = 2f;
            float startTime = 0f;
            if (musicSoundEmitter != null && musicSoundEmitter.IsPlaying())
            {
                AudioClip songToPlay = audioCue.GetClips()[0];
                if (musicSoundEmitter.GetClip() == songToPlay)
                    return AudioCueKey.Invalid;

                //Music is already playing, need to fade it out
                startTime = musicSoundEmitter.FadeMusicOut(fadeDuration);
            }
            musicSoundEmitter = pool.Request();
            musicSoundEmitter.FadeMusicIn(audioCue.GetClips()[0], audioConfiguration, 1f, startTime);
            musicSoundEmitter.OnSoundFinishedPlaying += StopMusicEmitter;
            return AudioCueKey.Invalid;
        }

        private void StopMusicEmitter(SoundEmitter soundEmitter)
        {
            soundEmitter.OnSoundFinishedPlaying -= StopMusicEmitter;
            pool.Return(soundEmitter);
        }

        private void OnSoundEmitterFinishedPlaying(SoundEmitter amitterToReturn)
        {
            pool.Return(amitterToReturn);
        }

        private IEnumerator WaitForClipEnd(SoundEmitter emitter, AudioClip clip)
        {
            yield return new WaitForSeconds(clip.length);
            pool.Return(emitter);
        }
        
        
    }
}