using Utilities.ImprovedTimers;
using Platformer;
using UnityEngine;

namespace State.EnemyStates
{
    public class EnemyIdleState: EnemyBaseState
    {
        private readonly CountdownTimer idleTimer;
        public EnemyIdleState(Enemy enemy, Animator animator, CountdownTimer idleTimer) : base(enemy, animator)
        {
            this.idleTimer = idleTimer;
        }

        public override void OnEnter()
        {
            animator.CrossFade(IdleHash,crossFadeDuration);
            idleTimer.Start();
        }
    }
}