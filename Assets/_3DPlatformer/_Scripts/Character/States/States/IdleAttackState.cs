using AdvancePlayerController;
using UnityEngine;

namespace State
{
    public class IdleAttackState : BaseState
    {
        public IdleAttackState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(WalkHash,false);
            animator.SetBool(AttackHash,true);
            
            player.ClearInputCache();
            player.StopMovement();
        }
    }
}