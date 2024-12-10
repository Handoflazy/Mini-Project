using AdvancePlayerController;
using UnityEngine;

namespace State
{
    public class IdleState : BaseState
    {
        public IdleState(Protagonist player, Animator animator) : base(player, animator)
        {
        }
        public override void OnEnter()
        {
            player.OnGroundContactRegained();
            animator.SetBool(WalkHash,false);
            animator.SetBool(AttackHash,false);
            animator.SetBool(AirboneHash,false);
            animator.SetBool(SurprisedHash,false);
            player.ClearInputCache();
            player.StopMovement();
        }
    }
}