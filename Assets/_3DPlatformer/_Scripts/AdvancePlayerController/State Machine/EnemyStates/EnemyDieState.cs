using Platformer;
using UnityEngine;

namespace AdvancePlayerController.State_Machine.EnemyStates
{
    public class EnemyDieState : EnemyBaseState
    {
        public EnemyDieState(Enemy enemy, Animator animator) : base(enemy, animator)
        {
        }

        public override void OnEnter()
        {
            
            Debug.Log("Enemy Die");
            animator.CrossFade(DieHash,crossFadeDuration);
        }
    }
}