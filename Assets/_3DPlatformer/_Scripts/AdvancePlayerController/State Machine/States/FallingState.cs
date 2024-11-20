using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class FallingState : BaseState
    {
        public FallingState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            
        }

        public override void OnExit()
        { 
            if(player.GetInputVelocity().magnitude>0.1)
            {
                animator.CrossFade(locomotionHash,CROSS_FADE_DURATION);
            }
            else
            {
                animator.CrossFade(LandHash,CROSS_FADE_DURATION);
            }
            animator.SetBool(AirHash,false);
        }
    }
}