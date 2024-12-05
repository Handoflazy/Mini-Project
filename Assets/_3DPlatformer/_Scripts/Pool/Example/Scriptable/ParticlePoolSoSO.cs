using Platformer.Factory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Pool.Example
{
    [CreateAssetMenu(fileName = "New Particle Pool", menuName = "Pool/Particle Pool")]
    public class ParticlePoolSoSO: ComponentPoolSO<PoolableParticle>
    {
        [FormerlySerializedAs("factory")] [SerializeField]
        private ParticleFactorySO factorySo;
        [SerializeField]
        private int initialPoolSize;

        public  IFactory<PoolableParticle> FactorySo
        {
            get { return factorySo; }
            set { factorySo = value as ParticleFactorySO; }
        }
        
        public override int InitialPoolSize
        {
            get => initialPoolSize;
            set => initialPoolSize = value;
        }
    }
}