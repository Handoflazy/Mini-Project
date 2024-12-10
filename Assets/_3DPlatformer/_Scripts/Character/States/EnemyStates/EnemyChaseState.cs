using Platformer;
using UnityEngine;
using UnityEngine.AI;

namespace State.EnemyStates
{
    public class EnemyChaseState: EnemyBaseState
    {
        readonly NavMeshAgent agent;
        readonly Transform player;
        public EnemyChaseState(Enemy enemy, Animator animator, Transform player, NavMeshAgent agent) : base(enemy, animator)
        {
            this.player = player;
            this.agent = agent;
        }
        public override void OnEnter() {
            Debug.Log("Chase");
            animator.CrossFade(RunHash, crossFadeDuration);
        }
        
        public override void Update() {
            Debug.Log(player);
            agent.SetDestination(player.position);
        }
    }
}