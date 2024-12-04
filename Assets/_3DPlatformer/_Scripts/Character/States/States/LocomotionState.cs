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

        public override void OnEnter() => player.OnGroundContactRegained();

        public override void Update()
        {
            var playerSpeed = player.GetMovementVelocity().magnitude;
            smoothSpeed = Mathf.SmoothStep(smoothSpeed, playerSpeed, 0.5f); //TODO: speedMultilier
            animator.SetFloat(SpeedHash,smoothSpeed);
            animator.SetBool(WalkHash,playerSpeed>0.01f);
        }
    }
}