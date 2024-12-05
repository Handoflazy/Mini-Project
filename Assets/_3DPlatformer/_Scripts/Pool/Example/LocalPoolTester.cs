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

        private ParticlePoolSoSO poolSoSo;
        private ParticleFactorySO factorySo;

        private IEnumerator Start()
        {
            factorySo = ScriptableObject.CreateInstance<ParticleFactorySO>();
            factorySo.Prefab = prefab;
            poolSoSo = ScriptableObject.CreateInstance<ParticlePoolSoSO>();
            poolSoSo.name = gameObject.name;
            poolSoSo.FactorySo = factorySo;
            poolSoSo.InitialPoolSize = InitialPoolSize;
            List<PoolableParticle> particles = poolSoSo.Request(10) as List<PoolableParticle>;
            foreach (PoolableParticle particle in particles)
            {
                particle.transform.position = Random.insideUnitSphere * 5f;
                particle.Play();
            }

            yield return new WaitForSeconds(5f);
            poolSoSo.Return(particles);

        }
    }
}