using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Utilities.Event_System.EventChannel;

namespace Platformer.AdvancePlayerController
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private int maxHealh = 100;
        [SerializeField] private FloatEventChannel playerHealthChannel;
        [SerializeField]
        private int currentHealth;

        public UnityEvent OnDie = new UnityEvent();
        public UnityEvent OnHit = new UnityEvent();
        public int CurrentHealth
        {
            get => currentHealth;
            set
            {
                currentHealth = value;
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealh);
                PublishHealthPercentage();
                if (IsDead)
                {
                    OnDie.Invoke();
                }
                
            }
        }

        public bool IsDead => currentHealth <= 0;

        private void Awake()
        {
            CurrentHealth = maxHealh;
        }

        public void TakeDamage(int damage)
        {
            if (IsDead)
                return;
            CurrentHealth -= damage;
            OnHit.Invoke();
            
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