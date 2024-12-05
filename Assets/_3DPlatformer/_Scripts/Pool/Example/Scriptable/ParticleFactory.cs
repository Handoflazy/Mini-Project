using Platformer.Factory;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Pool.Example
{
    [CreateAssetMenu(fileName = "New Particle Factory", menuName = "Factory/Particle Factory")]
    public class ParticleFactory : ComponentFactorySO<PoolableParticle>
    {
        [SerializeField]
        PoolableParticle prefab = default;

        public override PoolableParticle Prefab
        {
            get => prefab;
            set => prefab = value;
        }
    }
}