using Platformer.Factory;
using Platformer.Systems.AudioSystem;
using UnityEngine;

namespace Platformer.Pool.Example
{
    [CreateAssetMenu(fileName = "New SoundEmitter Pool", menuName = "Pool/SoundEmitter Pool")]
    public class SoundEmitterPoolSO : ComponentPoolSO<SoundEmitter>
    {
        [SerializeField] private SoundEmitterFactory factory;

        public override IFactory<SoundEmitter> Factory
        {
            get { return factory; }
            set { factory = value as SoundEmitterFactory; }
        }
    }
}