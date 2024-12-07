using AdvancePlayerController;
using Platformer._3DPlatformer._Scripts.Character;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class JumpState : BaseState
    {
        private PlayerEffectController dustController;
        public JumpState(Protagonist player, Animator animator, PlayerEffectController dustController) : base(player, animator)
        {
            this.dustController = dustController;
        }

        public override void OnEnter()
        {
            player.JumpStart();
            animator.SetBool(AirHash,true);
            dustController.PlayJumpParticles();
            player.OnGroundContactLost();
        }
        public override void OnExit()
        {
            animator.SetBool(AirHash,false);
        }
    }
}