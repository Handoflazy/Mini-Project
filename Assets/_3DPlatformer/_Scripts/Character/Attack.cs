using System;
using Character;
using Platformer.GamePlay;
using UnityEngine;
using Utilities.ImprovedTimers;

namespace Platformer._3DPlatformer._Scripts.Character
{
    public class Attack: MonoBehaviour
    {
        [SerializeField] private AttackConfigSO attackConfigSO;
        [SerializeField] private GameStateSO gameState;
        private CountdownTimer combatTimer;
        

        private void Start()
        {
            combatTimer = new CountdownTimer(30);
            combatTimer.OnTimerStart += () => gameState.UpdateGameState(GameState.Combat);
            combatTimer.OnTimerStop += () => gameState.UpdateGameState(GameState.Gameplay);
        }
        public AttackConfigSO AttackConfig => attackConfigSO;
        

        private void Awake()
        {
            gameObject.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            
            if (!other.CompareTag(gameObject.tag))
            {
                if (other.TryGetComponent(out Damageable damageableComp))
                {
                    if (!damageableComp.GetHit)
                    {
                        damageableComp.ReceiveAnAttack(attackConfigSO.AttackStrength);
                        
                        combatTimer.Start();
                    }
                }
            }
        }
        
        
    }
}