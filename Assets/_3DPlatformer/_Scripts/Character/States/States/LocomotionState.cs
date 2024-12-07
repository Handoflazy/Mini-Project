using AdvancePlayerController;
using Platformer._3DPlatformer._Scripts.Character;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class LocomotionState : BaseState
    {
        private float smoothSpeed = 0;
        private PlayerEffectController dustController;
        public LocomotionState(Protagonist player, Animator animator, PlayerEffectController dustController) : base(player, animator)
        {
            this.dustController = dustController;
        }

        public override void OnEnter()
        {
            dustController.EnableWalkParticles();
            player.OnGroundContactRegained();
        }

        public override void Update()
        {
            var playerSpeed = player.GetMovementVelocity().magnitude;
            smoothSpeed = Mathf.SmoothStep(smoothSpeed, playerSpeed, 0.5f); //TODO: speedMultilier
            animator.SetFloat(SpeedHash,smoothSpeed);
            animator.SetBool(WalkHash,playerSpeed>0.01f);
        }

        public override void OnExit()
        {
            dustController.DisableWalkParticles();
        }
    }
}