using Platformer;
using UnityEngine;

namespace State.EnemyStates
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