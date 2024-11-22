using Unity.Collections;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "PlayerHealth", menuName = "EntityConfig/Player's Health")]
    public class HealthSO : ScriptableObject
    {
        [Tooltip("The Initial health")] [SerializeField] [ReadOnly]
        private int maxHealth;

        [SerializeField] [ReadOnly] private int currentHealth;

        public int MaxHealth => maxHealth;
        public int CurrentHealth => currentHealth;
        
        public void SetMaxHealth(int newValue) => maxHealth = newValue;
        public void SetCurrentHealth(int newValue) => currentHealth = newValue;

        public void InflictDamage(int damageValue)
        {
            currentHealth -= damageValue;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }

        public void RestoreHealth(int healthValue)
        {
            currentHealth += healthValue;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        }
    }
}