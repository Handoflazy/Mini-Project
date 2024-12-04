using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public abstract class BaseState : IState
    {
        protected readonly Protagonist player;
        protected readonly Animator animator;
        protected static readonly int AirHash = Animator.StringToHash("IsAirbome");
        protected static readonly int SpeedHash = Animator.StringToHash("MovingSpeed");
        protected static readonly int AttackHash = Animator.StringToHash("IsAttacking");
        protected static readonly int WalkHash = Animator.StringToHash("IsWalking");
        protected static readonly int HitHash = Animator.StringToHash("ReceiveHit");
        protected static readonly int DieHash = Animator.StringToHash("Die");
        protected static readonly int SurprisedHash = Animator.StringToHash("IsSurprised");

        protected const float CROSS_FADE_DURATION = 0.1f;

        protected BaseState(Protagonist player, Animator animator)
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