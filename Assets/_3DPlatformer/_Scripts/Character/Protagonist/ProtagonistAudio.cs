using Platformer.Systems.AudioSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer._3DPlatformer._Scripts.Character
{
    public class ProtagonistAudio : CharacterAudio
    {
        [Space(10)]
         [SerializeField] private AudioCueSO swing;
         [SerializeField] private AudioCueSO liftoff;
         [SerializeField] private AudioCueSO land;
         [SerializeField] private AudioCueSO objectPickup;
         [SerializeField] private AudioCueSO footsteps;
         [SerializeField] private AudioCueSO getHit;
         [SerializeField] private AudioCueSO die;
         [SerializeField] private AudioCueSO talk;
        
        public void PlayFootstep() => PlayAudio(footsteps, _audioConfig, transform.position);
        public void PlayJumpLiftoff() => PlayAudio(liftoff, _audioConfig, transform.position);
        public void PlayJumpLand() => PlayAudio(land, _audioConfig, transform.position);
        public void PlaySwing() => PlayAudio(swing, _audioConfig, transform.position);
        public void PlayObjectPickup() => PlayAudio(objectPickup, _audioConfig, transform.position);
        public void PlayGetHit() => PlayAudio(getHit, _audioConfig, transform.position);
        public void PlayDie() => PlayAudio(die, _audioConfig, transform.position);
        public void PlayTalk() => PlayAudio(talk, _audioConfig, transform.position);
    }
}