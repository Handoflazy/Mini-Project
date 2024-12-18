using System;
using UnityEngine;
using UnityEngine.Events;
using Utilities.EventChannel;

namespace Character
{
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private HealthConfigSO healthConfigSO;
        [SerializeField] private HealthSO currentHealthSO;

        [Header("Broadcasting On")]
        [SerializeField] private VoidEventChannel updateHealthUI;
        [SerializeField] private VoidEventChannel deathEvent;
        
        [field:SerializeField]
        public bool GetHit { get; set; }
        public bool IsDead { get; set; }

        private void Awake()
        {
            if (currentHealthSO == null) return;
            currentHealthSO = ScriptableObject.CreateInstance<HealthSO>();
            currentHealthSO.SetMaxHealth(healthConfigSO.InitialHealth);
            currentHealthSO.SetCurrentHealth(healthConfigSO.InitialHealth);

            if (updateHealthUI != null)
            {
                updateHealthUI.Invoke();
            }
            
        }

        private void Start()
        {
            IsDead = false; //TODO: REMOVE AFTER TEST;
        }

        public void Cure(int healthToAdd)
        {
            if (IsDead)
                return;
            currentHealthSO.RestoreHealth(healthToAdd);
            if(updateHealthUI!=null)
                updateHealthUI.Invoke();
        }
        public void ReceiveAnAttack(int damage)
        {
            if (IsDead)
                return;
            currentHealthSO.InflictDamage(damage);
            if(updateHealthUI!=null)
                updateHealthUI.Invoke();
            GetHit = true;
            if (currentHealthSO.CurrentHealth != 0) return;
            IsDead = true;
            
            if (deathEvent != null)
                deathEvent.Invoke();

            currentHealthSO.SetCurrentHealth(healthConfigSO.InitialHealth);

        }
        public void Kill()
        {
            Debug.Log("Who Kill");
            ReceiveAnAttack(currentHealthSO.CurrentHealth);
        }
        public void Revive()
        {
            currentHealthSO.SetCurrentHealth(healthConfigSO.InitialHealth);
		
            if (updateHealthUI != null)
                updateHealthUI.Invoke();
			
            IsDead = false;
        }
    }
}