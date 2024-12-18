using AdvancePlayerController;
using Platformer.Character;
using UnityEngine;

namespace State
{
    public class JumpAttacking: BaseState
    {
        private readonly PlayerEffectController dustController;
        public JumpAttacking(Protagonist player, Animator animator, PlayerEffectController dustController) : base(player, animator)
        {
            this.dustController = dustController;
        }

        public override void OnEnter()
        {
            animator.SetBool(AttackHash, true);
            animator.SetBool(AirboneHash,true);
            
            player.ClearInputCache();
            dustController.PlayJumpParticles();
            dustController.PlayLandParticles(1);
            
        }
    }
}