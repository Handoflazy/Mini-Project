using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Systems.AudioSystem
{
    [CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
    public class AudioCueSO : ScriptableObject
    {
        public bool looping = false;
         [SerializeField] private AudioClipsGroup[] audioClipGroups = default;
        public AudioClip[] GetClips()
        {
            int numberOfClips = audioClipGroups.Length;
            AudioClip[] resultingClips = new AudioClip[numberOfClips];
            for (int i = 0; i < numberOfClips; i++)
            {
                resultingClips[i] = audioClipGroups[i].GetNextClip();
            }
            return resultingClips;
        }
    }

    [Serializable]
    public class AudioClipsGroup
    {
        public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
        public AudioClip[] audioClips;
        private int nextClipToPlay = -1;
        private int lastClipPlayed = -1;
        public AudioClip GetNextClip()
        {
            // Fast out if there is only one clip to play
            if(audioClips.Length == 1)
                return audioClips[0];
            if(nextClipToPlay == -1)
            {
                // Index needs to be initialised: 0 if Sequential, random if otherwise
                nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
            }
            else
            {
                // Select next clip index based on the appropriate SequenceMode
                switch (sequenceMode)
                {
                    case SequenceMode.Random:
                        nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        break;
                    case SequenceMode.RandomNoImmediateRepeat:
                        do
                        {
                            nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        } while (nextClipToPlay == lastClipPlayed);
                        break;
                    case SequenceMode.Sequential:
                        nextClipToPlay = (int)Mathf.Repeat(nextClipToPlay++, audioClips.Length);
                        break;
                }
            }
            lastClipPlayed = nextClipToPlay;
            return audioClips[nextClipToPlay];
        }
        public enum SequenceMode
        {
            Random,
            RandomNoImmediateRepeat,
            Sequential,
        }
    } 
}