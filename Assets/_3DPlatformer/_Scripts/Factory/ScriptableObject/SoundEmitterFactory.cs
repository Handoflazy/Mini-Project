using Platformer.Factory;
using Platformer.Systems.AudioSystem;
using UnityEngine;

namespace Platformer.Pool.Example
{
    [CreateAssetMenu(fileName = "New SoundEmitter Factory", menuName = "Factory/SoundEmitter Factory")]
    public class SoundEmitterFactory : FactorySO<SoundEmitter>
    {
        [SerializeField] private SoundEmitter prefab = default;
        public SoundEmitter Prefab
        {
            get => prefab;
            set => prefab = value;
        }
        public override SoundEmitter Create()
        {
            return Instantiate(prefab);
        }
    }
}