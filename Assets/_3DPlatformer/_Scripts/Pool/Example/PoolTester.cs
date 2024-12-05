using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Platformer.Pool.Example
{
    public class PoolTester: MonoBehaviour
    {
        [FormerlySerializedAs("poolSo")] [FormerlySerializedAs("pool")] [SerializeField] private ParticlePoolSoSO poolSoSo = default;

        private IEnumerator Start()
        {
            List<PoolableParticle> particles = poolSoSo.Request(10) as List<PoolableParticle>;
            foreach (var particle in particles)
            {
                particle.transform.position = Random.insideUnitSphere * 5f;
                particle.Play();
                yield return new WaitForSeconds(5f);
            }
            poolSoSo.Return(particles);
        }
    }
}