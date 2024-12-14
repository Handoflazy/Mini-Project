using Platformer.Systems.AudioSystem;
using UnityEngine;
using Utilities.Event_System.EventChannel;

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