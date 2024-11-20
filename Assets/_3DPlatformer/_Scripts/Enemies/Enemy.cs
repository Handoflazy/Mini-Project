using AdvancePlayerController;
using AdvancePlayerController.State_Machine;
using ImprovedTimers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity
    {
        [Header(" Elements ")] 
        [SerializeField, Required] private Animator animator;
        [SerializeField, Required] private NavMeshAgent navMeshAgent;
        [SerializeField, Required] private PlayerDetector detector;
        
        
        [SerializeField] float wanderRadius = 10f;
        [SerializeField] float idleTime = 3f;
        [SerializeField] float attackTime = 3f;
        
        
        private StateMachine stateMachine;
        private CountdownTimer waitTimer;
        private CountdownTimer attackTimer;

        private void Start()
        {
            SetUpTimers();

            SetUpStateMachine();
            
        }

        private void SetUpTimers()
        {
            waitTimer = new CountdownTimer(idleTime);
            attackTimer = new CountdownTimer(attackTime);
        }

        private void SetUpStateMachine()
        {
            /*stateMachine = new StateMachine();
            var wanderState = new EnemyWanderState(this, animator, navMeshAgent, wanderRadius);
            var chaseState = new EnemyChaseState.Builder()
                .WithAnimator(animator)
                .WithEnemy(this)
                .WithPlayerTransform(detector.player)
                .WithAgent(navMeshAgent)
                .Build();
            
            var idleState = new EnemyIdleState(animator, this,waitTimer);
            var attackState = new EnemyAttackState.Builder()
                .WithAnimator(animator)
                .WithEnemy(this)
                .WithPlayerTransform(detector.player)
                .WithStoppingDistance(detector.attackRange)
                .WithNavMeshAgent(navMeshAgent).Build();
            
            At(wanderState,chaseState, new FuncPredicate(()=>detector.CanDetectPlayer()));
            At(chaseState, wanderState, new FuncPredicate(()=>!detector.CanDetectPlayer()));
            At(wanderState,idleState,new FuncPredicate(()=>HasReachDestination()));
            At(idleState,wanderState,new FuncPredicate(()=>waitTimer.IsFinished));
            At(idleState,chaseState,new FuncPredicate(()=>detector.CanDetectPlayer()));
            At(chaseState, attackState, new FuncPredicate(() => detector.CanAttackPlayer()));
            At(attackState, chaseState, new FuncPredicate(() => !detector.CanAttackPlayer()));
            stateMachine.SetState(wanderState);*/
        }

        void At(IState from, IState to, IPredicate codition) => stateMachine.AddTransition(from, to, codition);
        void Any(IState to, IPredicate codition) =>stateMachine.AddAnyTransition(to, codition);

        private void Update()
        {
             stateMachine.Update();
             
        }

        private void FixedUpdate() => stateMachine.FixedUpdate();
        
        private bool HasReachDestination()
        {
            return !navMeshAgent.pathPending
                   && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                   && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f);
        }
        

        public void Attack()
        {
            if (attackTimer.IsRunning&&!detector.playerHealth.isDead)
                return;
            attackTimer.Start();
            detector.playerHealth.TakeDame(10);
            
           animator.Play("Slash Attack",0,0);
        }
        
    }
}
