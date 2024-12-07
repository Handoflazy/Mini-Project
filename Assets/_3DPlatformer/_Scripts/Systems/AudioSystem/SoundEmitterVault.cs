using System.Collections.Generic;

namespace Platformer.Systems.AudioSystem
{
    public class SoundEmitterVault
    {
        private int nextUniqueKey = 0;
        private List<AudioCueKey> emittersKey;
        private List<SoundEmitter[]> emittersList;
        public SoundEmitterVault()
        {
            emittersKey = new List<AudioCueKey>();
            emittersList = new List<SoundEmitter[]>();
        }
        public AudioCueKey GetKey(AudioCueSO cue)
        {
            return new AudioCueKey(nextUniqueKey++, cue);
        }
        public void Add(AudioCueKey key, SoundEmitter[] emitter)
        {
            emittersKey.Add(key);
            emittersList.Add(emitter);
        }
        public AudioCueKey Add(AudioCueSO cue, SoundEmitter[] emitter)
        {
            AudioCueKey emitterKey = GetKey(cue);

            emittersKey.Add(emitterKey);
            emittersList.Add(emitter);

            return emitterKey;
        }
        public bool Get(AudioCueKey key, out SoundEmitter[] emitter)
        {
            int index = emittersKey.FindIndex(x => x == key);

            if (index < 0)
            {
                emitter = null;
                return false;
            }

            emitter = emittersList[index];
            return true;
        }
        public bool Remove(AudioCueKey key)
        {
            int index = emittersKey.FindIndex(x => x == key);
            return RemoveAt(index);
        }

        private bool RemoveAt(int index)
        {
            if (index < 0)
            {
                return false;
            }

            emittersKey.RemoveAt(index);
            emittersList.RemoveAt(index);

            return true;
        }
    }
}