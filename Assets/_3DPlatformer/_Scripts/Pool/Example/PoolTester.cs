using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Pool.Example
{
    public class PoolTester: MonoBehaviour
    {
        [SerializeField] private ParticlePool pool = default;

        private IEnumerator Start()
        {
            List<PoolableParticle> particles = pool.Request(10) as List<PoolableParticle>;
            foreach (var particle in particles)
            {
                particle.transform.position = Random.insideUnitSphere * 5f;
                particle.Play();
                yield return new WaitForSeconds(5f);
            }
            pool.Return(particles);
        }
    }
}