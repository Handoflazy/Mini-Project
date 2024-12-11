using AdvancePlayerController;
using Platformer._3DPlatformer._Scripts.Character;
using UnityEngine;

namespace State
{
    public class WalkState: BaseState
    {
        private float smoothSpeed = 0;
        private readonly PlayerEffectController dustController;
        public WalkState(Protagonist player, Animator animator, PlayerEffectController dustController) : base(player, animator)
        {
            this.dustController = dustController;
        }

        public override void OnEnter()
        {
            player.ClearInputCache();
            player.OnGroundContactRegained();
            animator.SetBool(AttackHash,false);
            animator.SetBool(WalkHash,true);
            dustController.EnableWalkParticles();
        }

        public override void Update()
        {
            var playerSpeed = player.GetMovementVelocity().magnitude;
            smoothSpeed = Mathf.SmoothStep(smoothSpeed, playerSpeed, 0.5f); //TODO: speedMultilier
            animator.SetFloat(SpeedHash,smoothSpeed);
        }

        public override void OnExit()
        {
            animator.SetBool(WalkHash,false);
            dustController.DisableWalkParticles();
        }
    }
}