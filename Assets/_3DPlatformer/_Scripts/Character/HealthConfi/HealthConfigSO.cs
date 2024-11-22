using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "HealthConfig", menuName = "EntityConfig/Health Config")]
    public class HealthConfigSO : ScriptableObject
    {
        [SerializeField] private int initialHealth;
        public int InitialHealth => initialHealth;
    }
}