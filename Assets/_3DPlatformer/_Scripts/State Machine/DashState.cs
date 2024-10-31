using UnityEngine;

namespace Platformer.State_Machine
{
    public class DashState : BaseState
    {
        public DashState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Is Dashing");
            _animator.CrossFade(RunHash,CROSS_FADE_DURATION);
        }

        public override void FixedUpdate()
        {
            _player.HandleDash();
        }
    }
}