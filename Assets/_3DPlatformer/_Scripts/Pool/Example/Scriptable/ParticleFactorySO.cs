using Platformer.Factory;
using UnityEngine;

namespace Platformer.Pool.Example
{
    [CreateAssetMenu(fileName = "New Particle Factory", menuName = "Factory/Particle Factory")]
    public class ParticleFactorySO : ComponentFactory<PoolableParticle>
    {
        [SerializeField]
        private PoolableParticle prefab = default;

        public override PoolableParticle Prefab
        {
            get => prefab;
            set => prefab = value;
        }
    }
}