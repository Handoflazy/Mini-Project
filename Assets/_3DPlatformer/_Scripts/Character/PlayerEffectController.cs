using System.Collections;
using UnityEngine;

namespace Platformer._3DPlatformer._Scripts.Character
{
    public class PlayerEffectController: MonoBehaviour
    {
        [SerializeField] ParticleSystem walkingParticles = default;
        [SerializeField] ParticleSystem landParticles = default;
        [SerializeField] ParticleSystem jumpParticles = default;

        [SerializeField] ParticleSystem slashEffect = default;
        [SerializeField] ParticleSystem reverseSlashEffect = default;
        
        private void Start()
        {
            slashEffect.Stop();
            reverseSlashEffect.Stop();
        }
        public void EnableWalkParticles()
        {
            walkingParticles.Play();
        }
        
        public void DisableWalkParticles()
        {
            walkingParticles.Stop();
        }
        
        public void PlayJumpParticles()
        {
            jumpParticles.Play();
        }
        public void PlayLandParticles()
        {
            landParticles.Play();
        }
        
        public void PlaySlashEffect()
        {
            slashEffect.Play();
        }

        public void PlayReverseSlashEffect()
        {
            reverseSlashEffect.Play();
        }
        
        public void PlayLandParticles(float intensity)
        {
            // make sure intensity is always between 0 and 1
            intensity = Mathf.Clamp01(intensity);

            ParticleSystem.MainModule main = landParticles.main;
            ParticleSystem.MinMaxCurve origCurve = main.startSize; //save original curve to be assigned back to particle system
            ParticleSystem.MinMaxCurve newCurve = main.startSize; //Make a new minMax curve and make our changes to the new copy

            float minSize = newCurve.constantMin;
            float maxSize = newCurve.constantMax;

            // use the intensity to change the maximum size of the particle curve
            newCurve.constantMax = Mathf.Lerp(minSize, maxSize, intensity);
            main.startSize = newCurve;

            landParticles.Play();

            // Put the original startSize back where you found it
            StartCoroutine(ResetMinMaxCurve(landParticles, origCurve));

            // Note: We don't necessarily need to reset the curve, as it will be overridden
        }
        private IEnumerator ResetMinMaxCurve(ParticleSystem ps, ParticleSystem.MinMaxCurve curve)
        {
            while (ps.isEmitting)
            {
                yield return null;
            }

            ParticleSystem.MainModule main = ps.main;
            main.startSize = curve;
        }
    }
}