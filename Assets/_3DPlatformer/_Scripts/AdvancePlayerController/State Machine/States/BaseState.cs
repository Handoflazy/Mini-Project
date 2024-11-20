using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController player;
        protected readonly Animator animator;
        
        protected static readonly int locomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
        protected static readonly int AirHash = Animator.StringToHash("IsInAir");
        protected static readonly int LandHash = Animator.StringToHash("Land");
        protected static readonly int SpeedHash = Animator.StringToHash("MoveSpeed");
        protected static readonly int AttackHash = Animator.StringToHash("Attack");

        protected const float CROSS_FADE_DURATION = 0.1f;

        protected BaseState(PlayerController player, Animator animator)
        {
            this.player = player;
            this.animator = animator;
        }

        public virtual void OnEnter()
        {
            Debug.Log("BaseState.OnEnter");
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
          
        }

        public virtual void OnExit()
        {
        }
    }
}