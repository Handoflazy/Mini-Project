using System;
using UnityEngine;
using UnityEngine.Events;

namespace Platformer.AdvancePlayerController
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int maxHealh = 100;
        [SerializeField] private FloatEventChannel playerHealthChannel;
        [SerializeField]
        private int currentHealth;
        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealh);
                PublishHealthPercentage();
                
            }
        }

        public bool isDead => currentHealth <= 0;

        private void Awake()
        {
            CurrentHealth = maxHealh;
        }

        public void TakeDame(int damage)
        {
            if (isDead)
                return;
            CurrentHealth -= damage;
            
        }

        void PublishHealthPercentage()
        {
            if (playerHealthChannel != null)
            {
                playerHealthChannel.Invoke(CurrentHealth/(float)maxHealh);
            }
        }
    }
}