using System;
using Character;
using UnityEngine;

namespace Platformer._3DPlatformer._Scripts.Character
{
    public class Attack: MonoBehaviour
    {
        [SerializeField] private AttackConfigSO attackConfigSO;

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
                        damageableComp.ReceiveAnAttack(attackConfigSO.AttackStrength);
                }
            }
        }
        
        
    }
}