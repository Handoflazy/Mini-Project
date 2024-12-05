using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool.Example
{
    [CreateAssetMenu(fileName = "New Particle Factory", menuName = "Factory/Particle Factory")]
    public class ParticleFactory : ComponentFactorySO<PoolableParticle>
    {
        [SerializeField]
        PoolableParticle _prefab = default;

        public override PoolableParticle Prefab
        {
            get => _prefab;
            set => _prefab = value;
        }
    }
}