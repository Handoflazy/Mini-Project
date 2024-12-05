using Platformer.Factory;
using UnityEngine;
namespace Platformer.Pool.Example
{
    [CreateAssetMenu(fileName = "New Particle Pool", menuName = "Pool/Particle Pool")]
    public class ParticlePool: ComponentPool<PoolableParticle>
    {
        [SerializeField]
        private ParticleFactory factory;
        [SerializeField]
        private int initialPoolSize;

        public override IFactory<PoolableParticle> Factory
        {
            get { return factory; }
            set { factory = value as ParticleFactory; }
        }
        
        public override int InitialPoolSize
        {
            get
            {
                return initialPoolSize;
            }
            set
            {
                initialPoolSize = value;
            }
        }
    }
}