using Sirenix.OdinInspector;
using UnityEngine;

namespace Platformer.AdvancePlayerController.PlayerParticleSystem
{
    public class PlayerParticles : MonoBehaviour
    {
        [SerializeField, Required] private ParticleSystem slashVFX;

        public void PlaySlash()
        {
            slashVFX.Play();
        }
    }
}