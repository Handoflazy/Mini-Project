using Platformer.Systems.AudioSystem;
using UnityEngine;
using Utilities.EventChannel;

namespace Platformer._3DPlatformer._Scripts.Character
{
    public class CharacterAudio : MonoBehaviour
    {
        [SerializeField] protected AudioCueEventChannelSO _sfxEventChannel = default;
        [SerializeField] protected AudioConfigurationSO _audioConfig = default;
        //[SerializeField] protected GameStateSO _gameState = default;
	
        protected void PlayAudio(AudioCueSO audioCue, AudioConfigurationSO audioConfiguration, Vector3 positionInSpace = default)
        {
            //if (_gameState.CurrentGameState != GameState.Cutscene)
            _sfxEventChannel.RaisePlayEvent(audioCue, audioConfiguration, positionInSpace);
        }
    }
}