using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Platformer.Systems.AudioSystem
{
    public class AudioSystem : MonoBehaviour
    {
        private bool sfxMute;
        private bool musicMute;

        private AudioSource theme;
        private AudioSource sfx;

        private static readonly List<string> mixBuffer = new();
        private const float mixBufferClearDelay = 0.05f;

        private string currentTrack;
        
        public float delayInCrossfading = 0.3f;
        public List<Theme> tracks = new List<Theme>();
        public List<SFX> sounds = new List<SFX>();
        
        private static int BoolToBinary(bool b) => b ? 1 : 0;
        
        private SFX GetSoundByName(string sName) => sounds.Find(x => x.name == sName);
        public float MusicVolume => PlayerPrefs.GetFloat("MusicVolume",1f);
        public float SfxVolume => PlayerPrefs.GetFloat("SFXVolume",1f);


        private void Awake()
        {
            theme = gameObject.GetOrAdd<AudioSource>();
            theme.loop = true;
            sfx = gameObject.AddComponent<AudioSource>();
            sfxMute = false;
            musicMute = false;
            
            sfx.volume = Mathf.Abs(SfxVolume) > 0.05f ? SfxVolume : 0;
            theme.volume = Mathf.Abs(MusicVolume) > 0.05f ? SfxVolume : 0;
            
            if (PlayerPrefs.GetInt("sfxMute") == 1)
            {
                SfxToggle();
            }
            // Checks If The musicMute Is True Or Not
            if (PlayerPrefs.GetInt("musicMute") == 1)
            {
                MusicToggle();
            }
            StartCoroutine(MixBufferRoutine());


        }

        private void Start() //TODO REMOVE
        {
            PlayMusic(tracks[0].name);
        }

        IEnumerator MixBufferRoutine()
        {
            float time = 0;

            while (true)
            {
                time += Time.unscaledDeltaTime;
                yield return 0;
                if (time >= mixBufferClearDelay)
                {
                    mixBuffer.Clear();
                    time = 0;
                }
            }
        }

        public void PlayMusic(string trackName)
        {
            if (trackName != "")
                currentTrack = trackName;
            AudioClip to = tracks.Find(t => t.name == trackName).track;
            StartCoroutine(CrossFade(to));
            theme.Play();

        }
        public void StopSound()
        {
            sfx.Stop();
        }
        IEnumerator CrossFade(AudioClip to)
        {
            if (theme.clip != null)
            {
                while (delayInCrossfading > 0)
                {
                    theme.volume = delayInCrossfading * MusicVolume;
                    delayInCrossfading -= Time.unscaledDeltaTime;
                    yield return 0;
                }
            }
            theme.clip = to;
            if (to == null)
            {
                theme.Stop();
                yield break;
            }
            delayInCrossfading = 0;
            
            while (delayInCrossfading < 1f)
            {
                theme.volume = delayInCrossfading * MusicVolume;
                delayInCrossfading += Time.unscaledDeltaTime;
                yield return 0;
            }
            theme.volume = MusicVolume;
        }
        private void MusicToggle()
        {
            musicMute = !musicMute;
            theme.mute = musicMute;
            PlayerPrefs.SetInt("musicMute", BoolToBinary(musicMute));
            PlayerPrefs.Save();
        }

        private void SfxToggle()
        {
            sfxMute = !sfxMute;
            sfx.mute = sfxMute;
            PlayerPrefs.SetInt("sfxMute", BoolToBinary(sfxMute));
            PlayerPrefs.Save();
        }
        public void PlaySound(string clip)
        {
            SFX sound = GetSoundByName(clip);

            if (sound != null && !mixBuffer.Contains(clip))
            {
                if (sound.clips.Count == 0)
                    return;
                mixBuffer.Add(clip);
                sfx.PlayOneShot(sound.clips[Random.Range(0, sound.clips.Count - 1)]);
            }
        }
        
    }
    
    [Serializable]
    public class Theme
    {
        public string name;
        public AudioClip track;
    }

    [Serializable]
    public class SFX
    {
        public string name;
        public List<AudioClip> clips = new List<AudioClip>();
    }
}
