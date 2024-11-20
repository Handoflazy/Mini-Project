using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class SprintState : BaseState
    {
        public SprintState(PlayerController player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("dash");
            //animator.CrossFade(locomotionHash,CROSS_FADE_DURATION);
        }

        public override void FixedUpdate()
        {
            animator.SetFloat(SpeedHash,player.GetInputVelocity().magnitude);
        }
    }
}