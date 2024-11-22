using AdvancePlayerController;
using UnityEngine;

namespace AdvancePlayerController.State_Machine
{
    public class AttackState : BaseState
    {
        public AttackState(Protagonist player, Animator animator) : base(player, animator)
        {
        }

        public override void OnEnter()
        {
            animator.SetTrigger(AttackHash);
        }
    }
}