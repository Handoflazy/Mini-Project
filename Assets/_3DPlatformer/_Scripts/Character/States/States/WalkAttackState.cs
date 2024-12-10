using AdvancePlayerController;
using UnityEngine;

namespace State
{
    public class WalkAttackState : BaseState
    {
        private float smoothSpeed = 0;
        public WalkAttackState(Protagonist player, Animator animator) : base(player, animator)
        {
        }
        public override void OnEnter()
        {
            animator.SetBool(WalkHash,true);
            animator.SetBool(AttackHash,true);
            player.ClearInputCache();
        }

        public override void Update()
        {
            var playerSpeed = player.GetMovementVelocity().magnitude;
            smoothSpeed= Mathf.SmoothStep(smoothSpeed, playerSpeed, 0.5f); //TODO: speedMultilier
            animator.SetFloat(SpeedHash,smoothSpeed);
        }
        public override void OnExit()
        {
            animator.SetBool(AttackHash,false);
            animator.SetBool(WalkHash,false);
        }
    }
}