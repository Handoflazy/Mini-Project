using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.PlayerParticleSystem
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