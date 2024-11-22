using Platformer;
using UnityEngine;

namespace AdvancePlayerController.State_Machine.EnemyStates
{
    public class EnemyHitState: EnemyBaseState
    {
        protected EnemyHitState(Enemy enemy, Animator animator) : base(enemy, animator)
        {
        }

        public override void OnEnter()
        {
            animator.CrossFade(GetHitHash,crossFadeDuration);
        }
    }
}