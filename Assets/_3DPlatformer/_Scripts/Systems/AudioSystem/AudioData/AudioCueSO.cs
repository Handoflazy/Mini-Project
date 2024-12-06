using System;
using UnityEngine;

namespace Platformer.Systems.AudioSystem
{
    [CreateAssetMenu(fileName = "newAudioCue", menuName = "Audio/Audio Cue")]
    public class AudioCueSO : ScriptableObject
    {
        public bool looping = false;
        [SerializeField] private AudioClipsGroup[] _audioClipGroups = default;
        public AudioClip[] GetClips()
        {
            int numberOfClips = _audioClipGroups.Length;
            AudioClip[] resultingClips = new AudioClip[numberOfClips];
            for (int i = 0; i < numberOfClips; i++)
            {
                resultingClips[i] = _audioClipGroups[i].GetNextClip();
            }
            return resultingClips;
        }
    }

    [Serializable]
    public class AudioClipsGroup
    {
        public SequenceMode sequenceMode = SequenceMode.RandomNoImmediateRepeat;
        public AudioClip[] audioClips;
        private int _nextClipToPlay = -1;
        private int _lastClipPlayed = -1;
        public AudioClip GetNextClip()
        {
            // Fast out if there is only one clip to play
            if(audioClips.Length == 1)
                return audioClips[0];
            if(_nextClipToPlay == -1)
            {
                // Index needs to be initialised: 0 if Sequential, random if otherwise
                _nextClipToPlay = (sequenceMode == SequenceMode.Sequential) ? 0 : UnityEngine.Random.Range(0, audioClips.Length);
            }
            else
            {
                // Select next clip index based on the appropriate SequenceMode
                switch (sequenceMode)
                {
                    case SequenceMode.Random:
                        _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        break;
                    case SequenceMode.RandomNoImmediateRepeat:
                        do
                        {
                            _nextClipToPlay = UnityEngine.Random.Range(0, audioClips.Length);
                        } while (_nextClipToPlay == _lastClipPlayed);
                        break;
                    case SequenceMode.Sequential:
                        _nextClipToPlay = (int)Mathf.Repeat(_nextClipToPlay++, audioClips.Length);
                        break;
                }
            }
            _lastClipPlayed = _nextClipToPlay;
            return audioClips[_nextClipToPlay];
        }
        public enum SequenceMode
        {
            Random,
            RandomNoImmediateRepeat,
            Sequential,
        }
    } 
}