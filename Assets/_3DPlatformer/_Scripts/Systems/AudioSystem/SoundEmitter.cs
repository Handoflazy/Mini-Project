using System;
using System.Collections;
using DG.Tweening;
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
        public event UnityAction<SoundEmitter> OnSoundFinishedPlaying;

        private void Awake()
        {
            audioSource = this.GetOrAddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }

        public void PlayAudioClip(AudioClip clip,AudioConfigurationSO settings,bool hasToLoop, Vector3 position = default)
        {
            audioSource.clip = clip;
            settings.ApplyTo(audioSource);
            this.transform.position = position;
            audioSource.time = 0f;
            audioSource.loop = hasToLoop;

            audioSource.Play();
            if (!hasToLoop)
            {
                StartCoroutine(FinishedPlaying(clip.length));
            }
        }
        public void FadeMusicIn(AudioClip musicClip, AudioConfigurationSO settings, float duration, float startTime = 0f)
        {
            PlayAudioClip(musicClip, settings, true);
            audioSource.volume = 0f;
            if (startTime <= audioSource.clip.length)
                audioSource.time = startTime;

            audioSource.DOFade(settings.Volume, duration);
        }

        public float FadeMusicOut(float duration)
        {
            audioSource.DOFade(0f, duration).onComplete += OnFadeOutComplete;

            return audioSource.time;
        }

        private void OnFadeOutComplete()
        {
            NotifyBeingDone();
        }

        public AudioClip GetClip() => audioSource.clip;
        public void Resume() => audioSource.Play();

        public void Pause() => audioSource.Pause();

        private IEnumerator FinishedPlaying(float clipLength)
        {
            yield return new WaitForSeconds(clipLength);

            NotifyBeingDone();
        }

        private void NotifyBeingDone() => OnSoundFinishedPlaying.Invoke(this);


        public void Stop() => audioSource.Stop();

        public void Finish()
        {
            if (audioSource.loop)
            {
                audioSource.loop = false;
                float timeRemaining = audioSource.clip.length - audioSource.time;
                StartCoroutine(FinishedPlaying(timeRemaining));
            }
        }

        public bool IsPlaying() => audioSource.isPlaying;

        public bool IsLooping() => audioSource.loop;
    }
}