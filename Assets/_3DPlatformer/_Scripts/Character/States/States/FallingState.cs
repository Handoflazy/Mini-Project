using AdvancePlayerController;
using Platformer.Character;
using UnityEngine;
using Utilities.ImprovedTimers;

namespace State
{
    public class FallingState : BaseState
    {
        public FallingState(Protagonist player, Animator animator,PlayerEffectController dustController) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetBool(AttackHash,false);
        }

        public override void OnExit()
        {
            animator.SetBool(AirboneHash,false);
        }
    }
}