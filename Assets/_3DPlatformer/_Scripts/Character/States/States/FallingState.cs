using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class FallingState : BaseState
    {
        public FallingState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(AirHash,true);
        }

        public override void OnExit()
        {
            if (player.GetInputVelocity().magnitude > 0.1)
            {
                animator.SetBool(WalkHash,true);
            }

            animator.SetBool(AirHash,false);
        }
    }
}