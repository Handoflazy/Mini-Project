using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            player.OnJumpStart();
            animator.SetTrigger(JumpHash);
            player.OnGroundContactLost();
     
        }

        public override void FixedUpdate()
        {
           
        }
    }
}