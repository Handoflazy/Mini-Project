using System;
using System.Collections;
using UnityEngine;

namespace Platformer.Pool.Example
{
    public class PoolableParticle : MonoBehaviour, IPoolable
    {
        [SerializeField] private ParticleSystem particleSystem = default;
        public void OnRequest()
        {
            gameObject.SetActive(true);
        }

        public void Play()
        {
            particleSystem.Play();
        }
        public void OnReturn(Action onReset)
        {
            StartCoroutine(DoReset(onReset));
        }

        IEnumerator DoReset(Action onReset)
        {
            if (particleSystem.isPlaying)
            {
                yield return new WaitForSecondsRealtime(particleSystem.main.duration - (particleSystem.time %
                    particleSystem.main.duration));
                particleSystem.Stop();
                
            }

            yield return new WaitUntil(() => particleSystem.particleCount == 0);
            onReset.Invoke();
            gameObject.SetActive(false);
        }
    }
}