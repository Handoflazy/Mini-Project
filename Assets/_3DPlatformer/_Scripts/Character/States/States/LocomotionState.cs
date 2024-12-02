using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class LocomotionState : BaseState
    {
        private float smoothSpeed = 0;
        public LocomotionState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            
            player.OnGroundContactRegained();
        }

        public override void Update()
        {
            smoothSpeed = Mathf.SmoothStep(smoothSpeed, player.GetMovementVelocity().magnitude, 0.5f); //TODO: speedMultilier
            animator.SetFloat(SpeedHash,smoothSpeed);
        }

        public override void FixedUpdate()
        {
            
        }
    }
}