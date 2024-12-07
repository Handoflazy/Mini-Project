using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer._3DPlatformer._Scripts.Character
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "EntityConfig/Attack Config")]
    public class AttackConfigSO : ScriptableObject
    {
        [SerializeField] private int attackStrength;
        public int AttackStrength => attackStrength;
    }
}