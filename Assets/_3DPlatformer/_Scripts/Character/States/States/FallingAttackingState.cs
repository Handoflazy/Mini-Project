using AdvancePlayerController;
using UnityEngine;

namespace State
{
    public class FallingAttackingState : BaseState
    {
        public FallingAttackingState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(AttackHash, true);
        }
        public override void OnExit()   
        {
            animator.SetBool(AirboneHash,false);
        }
    }
}