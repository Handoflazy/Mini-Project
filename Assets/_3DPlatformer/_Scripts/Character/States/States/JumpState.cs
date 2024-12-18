using AdvancePlayerController;
using Platformer.Character;
using UnityEngine;

namespace State
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
            
            animator.SetBool(AttackHash,false);
            animator.SetBool(AirboneHash,true);
            
            dustController.PlayJumpParticles();
            dustController.PlayLandParticles(1);
            
            player.OnGroundContactLost();
        }
    }
}