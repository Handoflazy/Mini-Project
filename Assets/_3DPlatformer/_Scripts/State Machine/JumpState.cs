using UnityEngine;

namespace Platformer.State_Machine
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            _animator.CrossFade(JumpHash,CROSS_FADE_DURATION);
     
        }

        public override void FixedUpdate()
        {
            _player.HandleJump();
        }
    }
}