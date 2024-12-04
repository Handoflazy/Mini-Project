using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class JumpState : BaseState
    {
        public JumpState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            player.JumpStart();
            animator.SetBool(AirHash,true);
            player.OnGroundContactLost();
        }
        public override void OnExit()
        {
            animator.SetBool(AirHash,false);
        }
    }
}