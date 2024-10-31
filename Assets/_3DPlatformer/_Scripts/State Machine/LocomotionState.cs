using UnityEngine;

namespace Platformer.State_Machine
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(locomotionHash,CROSS_FADE_DURATION);
        }

        public override void FixedUpdate()
        {
            _player.HandleMovement();
        }
    }
}