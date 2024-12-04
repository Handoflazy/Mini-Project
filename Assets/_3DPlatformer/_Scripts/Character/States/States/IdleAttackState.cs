using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
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
            player.ClearInput();
            player.StopMovement();
        }

        public override void OnExit()
        {
            animator.SetBool(AttackHash,false);
        }
    }
}