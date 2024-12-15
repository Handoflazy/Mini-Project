using Platformer.Systems.AudioSystem;
using UnityEngine;
using Utilities.EventChannel;

namespace Platformer.CutScene
{
    public class CutsceneAudioCongiSetter : MonoBehaviour
    {
        [SerializeField] private AudioConfigurationSO audioConfig = default;
        
        //Void Listener
        private void SetVolume()
        {
            GetComponent<AudioSource>().volume = audioConfig.Volume;
        }
    }
}