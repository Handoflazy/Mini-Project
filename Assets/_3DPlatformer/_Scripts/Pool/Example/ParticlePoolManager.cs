using System.Collections;
using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool.Example
{
    public class ParticlePoolManager : MonoBehaviour
    {
        private Pool<PoolableParticle> pool;
        
        [SerializeField]
        PoolableParticle particlePrefab = default;
        [Range(.1f,10f)]
        [SerializeField]
        float secondsBetweenSpawn = 1f;
        private void Start()
        {
            pool = new Pool<PoolableParticle>(new ComponentFactory<PoolableParticle>(particlePrefab),5);
            StartCoroutine(SpawnFoo(secondsBetweenSpawn));
        }
        
        IEnumerator SpawnFoo(float delay)
        {
           
            PoolableParticle particle = pool.Request();
            particle.transform.position = Random.insideUnitSphere * 5f;
            particle.Play();
            pool.Return(particle);
            yield return new WaitForSecondsRealtime(delay);
            StartCoroutine(SpawnFoo(secondsBetweenSpawn));
        }
    }
}