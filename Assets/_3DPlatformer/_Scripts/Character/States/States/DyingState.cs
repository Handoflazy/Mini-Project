using AdvancePlayerController;
using UnityEngine;

namespace State
{
    public class DyingState : BaseState
    {
        public DyingState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(SurprisedHash,false);
            animator.SetBool(AttackHash,false);
            player.Die();
            animator.Play(DieHash);
        }
    }
}