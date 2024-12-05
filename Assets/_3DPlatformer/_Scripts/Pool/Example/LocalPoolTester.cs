using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Platformer.Pool.Example
{
    public class LocalPoolTester : MonoBehaviour
    {
        [SerializeField] private PoolableParticle prefab = default;
        [SerializeField] private int InitialPoolSize = 5;

        private ParticlePool pool;
        private ParticleFactory factory;

        private IEnumerator Start()
        {
            factory = ScriptableObject.CreateInstance<ParticleFactory>();
            factory.Prefab = prefab;
            pool = ScriptableObject.CreateInstance<ParticlePool>();
            pool.name = gameObject.name;
            pool.Factory = factory;
            pool.InitialPoolSize = InitialPoolSize;
            List<PoolableParticle> particles = pool.Request(10) as List<PoolableParticle>;
            foreach (PoolableParticle particle in particles)
            {
                particle.transform.position = Random.insideUnitSphere * 5f;
                particle.Play();
            }

            yield return new WaitForSeconds(5f);
            pool.Return(particles);

        }
    }
}