using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Character
{
    [CreateAssetMenu(fileName = "AttackConfig", menuName = "EntityConfig/Attack Config")]
    public class AttackConfigSO : ScriptableObject
    {
        [SerializeField] private int attackStrength;
        public int AttackStrength => attackStrength;
    }
}