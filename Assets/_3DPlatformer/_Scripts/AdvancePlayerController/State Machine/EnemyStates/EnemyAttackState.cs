using Utilities.ImprovedTimers;
using Platformer;
using UnityEngine;
using UnityEngine.AI;

namespace AdvancePlayerController.State_Machine.EnemyStates
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent agent;
        private readonly Transform player;
        private readonly float attackDistance;
        private readonly float defaultStoppingDistance;
        public EnemyAttackState(Enemy enemy, Animator animator, Transform player, NavMeshAgent agent, float attackDistance) : base(enemy, animator)
        {
            this.agent = agent;
            this.player = player;
            defaultStoppingDistance = agent.stoppingDistance;
            this.attackDistance = attackDistance;
        }
        
        public override void OnEnter() {
            animator.CrossFade(AttackHash, crossFadeDuration);
            agent.stoppingDistance = attackDistance;
        }
        
        public override void Update() {
            agent.SetDestination(player.position);
            enemy.Attack();
        }

        public override void OnExit()
        {
            agent.stoppingDistance = defaultStoppingDistance;
        }
    }
}