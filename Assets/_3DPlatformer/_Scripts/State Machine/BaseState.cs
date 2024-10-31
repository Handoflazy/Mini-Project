using UnityEngine;
namespace Platformer.State_Machine
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController _player;
        protected readonly Animator _animator;
        
        protected static readonly int locomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");
        protected static readonly int RunHash = Animator.StringToHash("Run");

        protected const float CROSS_FADE_DURATION = 0.1f;

        protected BaseState(PlayerController player, Animator animator)
        {
            _player = player;
            _animator = animator;
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
          Debug.Log("BaseState.OnExit");
        }
    }
}